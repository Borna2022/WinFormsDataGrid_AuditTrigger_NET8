using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp2.Data;
using WinFormsApp2.Models;
namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        private readonly MyDbContext context;
        private readonly BindingSource bindingSource = new BindingSource();

        public Form1()
        {
            InitializeComponent();

            context = new MyDbContext();

            dataGridView1.RowValidated += DataGridView1_RowValidated;
            dataGridView1.CellEndEdit += DataGridView1_CellEndEdit;
            dataGridView1.DataBindingComplete += DataGridView1_DataBindingComplete;
            dataGridView1.KeyDown += DataGridView1_KeyDown;

            this.FormClosed += Form1_FormClosed;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // بارگذاری با Tracking
            context.MixedZoneSteelGroups.Load();

            bindingSource.DataSource = context.MixedZoneSteelGroups.Local.ToBindingList();
            dataGridView1.DataSource = bindingSource;

            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
        }

        private void DataGridView1_DataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                if (col.DataPropertyName == "Fsg_GroupId" || col.DataPropertyName == "Fsg_Row")
                    col.ReadOnly = true;
            }
        }

        private void DataGridView1_CellEndEdit(object? sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.EndEdit();
            bindingSource.EndEdit();
            dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void DataGridView1_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                dataGridView1.EndEdit();
                bindingSource.EndEdit();
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
                // ذخیره‌سازی در RowValidated انجام می‌شود
            }
        }

        // ===== نقطه‌ی ذخیره: UPDATE دستی + Reload از DB + Refresh UI =====
        private void DataGridView1_RowValidated(object? sender, DataGridViewCellEventArgs e)
        {
            try
            {
                dataGridView1.EndEdit();
                bindingSource.EndEdit();
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);

                context.ChangeTracker.DetectChanges();

                var modifiedEntries = context.ChangeTracker
                    .Entries<MixedZoneSteelGroup>()
                    .Where(en => en.State == EntityState.Modified)
                    .ToList();

                if (modifiedEntries.Count == 0)
                    return;

                using var tx = context.Database.BeginTransaction();
                try
                {
                    // 1) اجرای UPDATE خام برای هر رکورد (بدون OUTPUT)
                    foreach (var entry in modifiedEntries)
                    {
                        var item = entry.Entity;
                        context.Database.ExecuteSqlInterpolated($@"
                            UPDATE TMixedZoneSteelGroup
                            SET Fsg_MixedZoneSteelGroup = {item.Fsg_MixedZoneSteelGroup}
                            WHERE Fsg_GroupId = {item.Fsg_GroupId} AND Fsg_Row = {item.Fsg_Row}");
                    }

                    // 2) Commit تراکنش تا تغییرات در DB نهایی شوند (مهم)
                    tx.Commit();

                    // 3) بعد از Commit، برای هر موجودیت Reload کن تا مقدار واقعی DB (که ممکن است توسط تریگر تغییر کرده) در EF قرار بگیرد
                    foreach (var entry in modifiedEntries)
                    {
                        var item = entry.Entity;
                        try
                        {
                            // Reload synchronous; اگر می‌خواهی async استفاده کن
                            context.Entry(item).Reload();
                        }
                        catch (Exception reloadEx)
                        {
                            _ = reloadEx; // استفاده از discard برای جلوگیری از هشدار
                            // اگر reload شکست خورد، می‌توانیم رکورد را مجدداً از DB query کنیم
                            var fresh = context.MixedZoneSteelGroups
                                .AsNoTracking()
                                .FirstOrDefault(x => x.Fsg_GroupId == item.Fsg_GroupId && x.Fsg_Row == item.Fsg_Row);
                            if (fresh != null)
                            {
                                // مقدار مورد نظر را روی entity فعلی قرار بده
                                item.Fsg_MixedZoneSteelGroup = fresh.Fsg_MixedZoneSteelGroup;
                            }

                        }

                        // علامت‌گذاری entry به Unchanged تا EF دوباره تلاش نکند
                        var tracked = context.ChangeTracker.Entries<MixedZoneSteelGroup>()
                            .FirstOrDefault(en => en.Entity.Fsg_GroupId == item.Fsg_GroupId && en.Entity.Fsg_Row == item.Fsg_Row);
                        if (tracked != null)
                            tracked.State = EntityState.Unchanged;
                    }

                    // 4) ری‌فِرش UI تا مقدار جدید نمایش داده شود
                    bindingSource.ResetBindings(false);

                    MessageBox.Show($"تغییرات ذخیره شد ({modifiedEntries.Count} رکورد). Trigger اجرا شد.");
                }
                catch (Exception ex)
                {
                    // اگر خطا در اجرای UPDATE یا Commit رخ داد، rollback کن
                    try { tx.Rollback(); } catch { }
                    MessageBox.Show("خطای ذخیره در دیتابیس: " + (ex.InnerException?.Message ?? ex.Message));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطا هنگام ذخیره‌سازی: " + (ex.InnerException?.Message ?? ex.Message));
            }
        }

        private void Form1_FormClosed(object? sender, FormClosedEventArgs e)
        {
            context?.Dispose();
        }
    }
}
