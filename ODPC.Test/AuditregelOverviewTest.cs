using Microsoft.EntityFrameworkCore;
using ODPC.Data;
using ODPC.Data.Entities;
using ODPC.Features;
using ODPC.Features.AuditregelOverzicht;

namespace ODPC.Test
{
    [TestClass]
    public class AuditregelOverviewTest
    {
        [DataTestMethod]
        [DataRow(12, 1, 12, false, false)]
        [DataRow(30, 1, 15, false, true)]
        [DataRow(45, 2, 15, true, true)]
        [DataRow(16, 2, 1, true, false)]
        public async Task Paginering_werkt(int totalRecords, int? page, int expectedResultsCount, bool expectedPrevious, bool expectedNext)
        {
            var input = Enumerable.Range(0, totalRecords).Select(_ => RandomAuditregel()).ToList();

            using var context = GetDbContext();
            await context.AddRangeAsync(input);
            await context.SaveChangesAsync();

            var currentUri = new CurrentRequestUri("https://example.com");

            var controller = new AuditregelsController(context, currentUri);

            var result = await controller.Get(new() { Page = page }, default);

            if (expectedPrevious)
            {
                Assert.IsNotNull(result.Previous);
            }
            else
            {
                Assert.IsNull(result.Previous);
            }

            if (expectedNext)
            {
                Assert.IsNotNull(result.Next);
            }
            else
            {
                Assert.IsNull(result.Next);
            }

            Assert.AreEqual(totalRecords, result.Count);
            Assert.AreEqual(expectedResultsCount, result.Results.Count);
        }

        [TestMethod]
        public async Task Filter_GebruikerId_Werkt()
        {
            var auditRegel1 = RandomAuditregel();
            var auditRegel2 = RandomAuditregel();

            using var context = GetDbContext();
            await context.AddRangeAsync(auditRegel1, auditRegel2);
            await context.SaveChangesAsync();

            var currentUri = new CurrentRequestUri("https://example.com");

            var controller = new AuditregelsController(context, currentUri);

            var result = await controller.Get(new() { GebruikersId = auditRegel1.GebruikersId }, default);

            Assert.AreEqual(1, result.Results.Count);
            Assert.AreEqual(auditRegel1.GebruikersId, result.Results[0].GebruikersId);
        }

        [TestMethod]
        public async Task Filter_EntiteitId_Werkt()
        {
            var auditRegel1 = RandomAuditregel();
            var auditRegel2 = RandomAuditregel();

            using var context = GetDbContext();
            await context.AddRangeAsync(auditRegel1, auditRegel2);
            await context.SaveChangesAsync();

            var currentUri = new CurrentRequestUri("https://example.com");

            var controller = new AuditregelsController(context, currentUri);

            var result = await controller.Get(new() { ResourceUuid = auditRegel1.ResourceUuid }, default);

            Assert.AreEqual(1, result.Results.Count);
            Assert.AreEqual(auditRegel1.ResourceUuid, result.Results[0].ResourceUuid);
        }

        [DataTestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public async Task Filter_Geslaagd_Werkt(bool geslaagd)
        {
            var auditRegel1 = RandomAuditregel();
            auditRegel1.Geslaagd = geslaagd;

            var auditRegel2 = RandomAuditregel();
            auditRegel2.Geslaagd = !geslaagd;

            using var context = GetDbContext();
            await context.AddRangeAsync(auditRegel1, auditRegel2);
            await context.SaveChangesAsync();

            var currentUri = new CurrentRequestUri("https://example.com");

            var controller = new AuditregelsController(context, currentUri);

            var result = await controller.Get(new() { Geslaagd = geslaagd }, default);

            Assert.AreEqual(1, result.Results.Count);
            Assert.AreEqual(geslaagd, result.Results[0].Geslaagd);
        }

        private static Auditregel RandomAuditregel() => new()
        {
            ResourceUuid = Guid.NewGuid(),
            Resource = Guid.NewGuid().ToString(),
            GebruikersId = Guid.NewGuid().ToString(),
            Actie = AuditregelActie.update,
            ActieWeergave = Guid.NewGuid().ToString(),
            ResultaatWeergave = Guid.NewGuid().ToString(),
            Resultaat = System.Net.HttpStatusCode.OK,
            Wijzigingen = new(),
        };

        private static OdpcDbContext GetDbContext() => new(new DbContextOptionsBuilder<OdpcDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
    }
}
