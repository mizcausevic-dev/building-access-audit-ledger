using BuildingAccessAuditLedger.Api;
using Microsoft.AspNetCore.Mvc.Testing;

namespace BuildingAccessAuditLedger.Tests;

public sealed class BuildingAccessAuditLedgerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public BuildingAccessAuditLedgerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Overview_route_renders_access_shell()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/");
        var html = await response.Content.ReadAsStringAsync();

        Assert.True(response.IsSuccessStatusCode);
        Assert.Contains("Building Access Audit Ledger", html);
        Assert.Contains("building access", html);
    }

    [Fact]
    public async Task Api_summary_returns_expected_counts()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/dashboard/summary");
        var json = await response.Content.ReadAsStringAsync();

        Assert.True(response.IsSuccessStatusCode);
        Assert.Contains("\"snapshots\":2", json);
        Assert.Contains("\"blockingGaps\":4", json);
    }

    [Fact]
    public void Analysis_flags_high_risk_access_gaps()
    {
        var report = AnalysisService.Analyze(SampleData.Payload);

        Assert.Equal(2, report.Snapshots);
        Assert.Equal(6, report.Gaps);
        Assert.Contains(report.Findings, finding => finding.Code == "controller-drift-gap");
        Assert.Contains(report.Findings, finding => finding.Code == "shared-credential-gap");
    }
}
