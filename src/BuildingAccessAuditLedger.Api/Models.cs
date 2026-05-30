namespace BuildingAccessAuditLedger.Api;

public sealed record AccessSnapshot(
    string Id,
    string Name,
    string AccessLane,
    string Site,
    string Status,
    string SnapshotStatus,
    string Owner,
    int OpenEvents,
    int BlockingEvents,
    DateTimeOffset CollectedAt
);

public sealed record AccessGap(
    string Id,
    string SnapshotId,
    string ControlFamily,
    string Severity,
    string Subject,
    string ExpectedState,
    string ObservedState,
    int HoursOpen,
    bool BlocksAccess
);

public sealed record AccessLanePacket(
    string Id,
    string Lane,
    string Owner,
    string Status,
    string Focus,
    string NextAction,
    string Note
);

public sealed record AuditPacket(
    string PacketId,
    string Lane,
    string Owner,
    string Status,
    int CompletenessScore,
    string Blocker,
    string DecisionNote,
    int ReviewWindowHours
);

public sealed record BuildingAccessAuditExport(
    IReadOnlyList<AccessSnapshot> Snapshots,
    IReadOnlyList<AccessGap> Gaps
);

public sealed record BuildingAccessAuditFinding(
    string Code,
    string Severity,
    string Subject,
    string Message,
    string Owner
);

public sealed record BuildingAccessPostureReport(
    int Snapshots,
    int CurrentSnapshots,
    int Gaps,
    int BlockingGaps,
    int ControlRisks,
    int AuditRisks,
    IReadOnlyList<BuildingAccessAuditFinding> Findings,
    bool Ok
);
