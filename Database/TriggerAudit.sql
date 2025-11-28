USE [Test]
GO

/****** Object:  Trigger [dbo].[trg_TMixedZoneSteelGroup_Audit]    Script Date: 11/28/2025 9:18:12 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[trg_TMixedZoneSteelGroup_Audit]
ON [dbo].[TMixedZoneSteelGroup]
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- INSERT operations
    INSERT INTO dbo.TMixedZoneSteelGroup_Audit (Fsg_GroupId, Fsg_Row, NewValue, OperationType, PerformedBy)
    SELECT 
        i.Fsg_GroupId,
        i.Fsg_Row,
        i.Fsg_MixedZoneSteelGroup,
        'INSERT',
        SYSTEM_USER
    FROM inserted i
    LEFT JOIN deleted d ON i.Fsg_GroupId = d.Fsg_GroupId AND i.Fsg_Row = d.Fsg_Row
    WHERE d.Fsg_GroupId IS NULL;

    -- UPDATE operations
    INSERT INTO dbo.TMixedZoneSteelGroup_Audit (Fsg_GroupId, Fsg_Row, OldValue, NewValue, OperationType, PerformedBy)
    SELECT 
        i.Fsg_GroupId,
        i.Fsg_Row,
        d.Fsg_MixedZoneSteelGroup,
        i.Fsg_MixedZoneSteelGroup,
        'UPDATE',
        SYSTEM_USER
    FROM inserted i
    INNER JOIN deleted d ON i.Fsg_GroupId = d.Fsg_GroupId AND i.Fsg_Row = d.Fsg_Row
    WHERE ISNULL(i.Fsg_MixedZoneSteelGroup, -1) <> ISNULL(d.Fsg_MixedZoneSteelGroup, -1);
END;
GO

ALTER TABLE [dbo].[TMixedZoneSteelGroup] ENABLE TRIGGER [trg_TMixedZoneSteelGroup_Audit]
GO

