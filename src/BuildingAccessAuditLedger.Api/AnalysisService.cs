namespace BuildingAccessAuditLedger.Api;

public static class AnalysisService
{
    public static BuildingAccessPostureReport Analyze(BuildingAccessAuditExport payload)
    {
        var findings = new List<BuildingAccessAuditFinding>();

        foreach (var gap in payload.Gaps)
        {
            findings.Add(new BuildingAccessAuditFinding(
                GapCode(gap.ControlFamily),
                gap.Severity,
                gap.Subject,
                gap.ObservedState,
                OwnerForGap(gap.ControlFamily)));
        }

        var snapshots = payload.Snapshots.Count;
        var currentSnapshots = payload.Snapshots.Count(snapshot => snapshot.SnapshotStatus == "CURRENT");
        var gaps = payload.Gaps.Count;
        var blockingGaps = payload.Gaps.Count(gap => gap.BlocksAccess);
        var controlRisks = payload.Gaps.Count(gap => gap.ControlFamily is "Approval" or "Controller" or "Vendor" or "Credential");
        var auditRisks = payload.Gaps.Count(gap => gap.ControlFamily is "Revocation" or "Audit");
        var ok = blockingGaps == 0;

        return new BuildingAccessPostureReport(
            snapshots,
            currentSnapshots,
            gaps,
            blockingGaps,
            controlRisks,
            auditRisks,
            findings,
            ok
        );
    }

    public static object Summary()
    {
        var report = Analyze(SampleData.Payload);
        return new
        {
            snapshots = report.Snapshots,
            currentSnapshots = report.CurrentSnapshots,
            gaps = report.Gaps,
            blockingGaps = report.BlockingGaps,
            controlRisks = report.ControlRisks,
            auditRisks = report.AuditRisks,
            ok = report.Ok
        };
    }

    private static string GapCode(string family) => family switch
    {
        "Approval" => "badge-approval-gap",
        "Controller" => "controller-drift-gap",
        "Vendor" => "vendor-access-window-gap",
        "Revocation" => "revocation-propagation-gap",
        "Audit" => "audit-trace-gap",
        "Credential" => "shared-credential-gap",
        _ => "building-access-control-gap"
    };

    private static string OwnerForGap(string family) => family switch
    {
        "Approval" => "Property Access Operations",
        "Controller" => "Building Systems Engineering",
        "Vendor" => "Facilities Operations",
        "Revocation" => "Security Operations",
        "Audit" => "Security Operations",
        "Credential" => "Property Access Operations",
        _ => "Property Access Operations"
    };
}
