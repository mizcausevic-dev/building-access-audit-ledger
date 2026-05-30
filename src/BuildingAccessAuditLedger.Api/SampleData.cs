namespace BuildingAccessAuditLedger.Api;

public static class SampleData
{
    public static readonly BuildingAccessAuditExport Payload = new(
        Snapshots:
        [
            new(
                "hq-access-core",
                "HQ access change snapshot",
                "Controller and tenant approval lane",
                "BOS-HQ-01",
                "WATCH",
                "CURRENT",
                "Property Access Operations",
                8,
                3,
                DateTimeOffset.Parse("2026-05-29T16:10:00Z")
            ),
            new(
                "tower-revoke-review",
                "Tower revocation snapshot",
                "Revocation and incident lane",
                "RTP-TWR-02",
                "CRITICAL",
                "STALE",
                "Security Operations",
                5,
                2,
                DateTimeOffset.Parse("2026-05-27T10:00:00Z")
            )
        ],
        Gaps:
        [
            new(
                "gap-badge-approval",
                "hq-access-core",
                "Approval",
                "high",
                "Tenant move-in badge packet",
                "New tenant badges keep owner approval, suite mapping, and effective dates before activation.",
                "The current badge packet is missing the final property-manager approval and move-in effective date.",
                18,
                true
            ),
            new(
                "gap-controller-drift",
                "hq-access-core",
                "Controller",
                "high",
                "Lobby controller drift",
                "Controller changes keep reviewed rule sets and a logged rollback note before sync to production doors.",
                "One lobby controller is running a rule set that differs from the approved baseline and has no rollback note.",
                21,
                true
            ),
            new(
                "gap-vendor-window",
                "tower-revoke-review",
                "Vendor",
                "medium",
                "After-hours vendor access window",
                "Vendor access windows stay tied to an active work order and auto-expire at the end of the approved window.",
                "One vendor credential stayed active after the work order ended and needs closure evidence.",
                14,
                false
            ),
            new(
                "gap-revocation-lag",
                "tower-revoke-review",
                "Revocation",
                "high",
                "Terminated contractor badge",
                "Revoked badges deactivate across all controllers inside the agreed review window.",
                "A terminated contractor badge still appears active on one secondary controller.",
                26,
                true
            ),
            new(
                "gap-audit-trace",
                "hq-access-core",
                "Audit",
                "medium",
                "Badge-event audit continuity",
                "Access events keep a continuous timestamped export before review closure.",
                "One event export omitted a supervisor approval event and needs regeneration.",
                11,
                false
            ),
            new(
                "gap-credential-proof",
                "tower-revoke-review",
                "Credential",
                "high",
                "Shared credential review",
                "Shared credentials keep named ownership, scope, and renewal proof before they remain active.",
                "A shared access credential lacks current ownership evidence and renewal approval.",
                16,
                true
            )
        ]
    );

    public static readonly IReadOnlyList<AccessLanePacket> AccessLane =
    [
        new(
            "move-in-approvals",
            "Tenant move-in lane",
            "Property Access Operations",
            "red",
            "Badge requests, suite mappings, and effective-date approval continuity",
            "Close the missing manager approval before activating the remaining tenant badges.",
            "Move-in posture is not safe enough for blind badge activation."
        ),
        new(
            "controller-governance",
            "Controller governance lane",
            "Building Systems Engineering",
            "red",
            "Controller drift, rollback notes, and production door synchronization",
            "Repair the rule-set drift and attach rollback proof before the next controller sync.",
            "One controller is outside the approved baseline and weakens access trust."
        ),
        new(
            "vendor-access",
            "Vendor access lane",
            "Facilities Operations",
            "yellow",
            "After-hours windows, work-order linkage, and expiration discipline",
            "Close the stale vendor window and confirm auto-expire behavior on the next review.",
            "Vendor posture is recoverable if the lingering window is closed quickly."
        ),
        new(
            "revocation-review",
            "Revocation and incident lane",
            "Security Operations",
            "red",
            "Badge revocation timing, controller propagation, and incident evidence",
            "Deactivate the terminated contractor badge across all controllers before the next audit checkpoint.",
            "Revocation lag is still blocking a credible access posture."
        )
    ];

    public static readonly IReadOnlyList<AuditPacket> AuditPackets =
    [
        new(
            "BALA-12",
            "Tenant badge approval packet",
            "Property Access Operations",
            "red",
            59,
            "Final property-manager approval and effective date are still missing.",
            "Do not activate the remaining tenant badges until the approval packet is complete.",
            7
        ),
        new(
            "BALA-18",
            "Controller drift packet",
            "Building Systems Engineering",
            "red",
            63,
            "One production controller is running outside the approved baseline.",
            "Block the next sync and reroute the controller through governed rollback review.",
            10
        ),
        new(
            "BALA-21",
            "Vendor access packet",
            "Facilities Operations",
            "yellow",
            78,
            "An after-hours vendor window remained active past the work-order close.",
            "Closure can recover if the lingering vendor credential is revoked in the next cycle.",
            13
        ),
        new(
            "BALA-27",
            "Revocation audit packet",
            "Security Operations",
            "yellow",
            74,
            "A revoked badge still appears on a secondary controller and audit continuity is incomplete.",
            "Access posture is recoverable if revocation proof lands before the next checkpoint.",
            18
        )
    ];
}
