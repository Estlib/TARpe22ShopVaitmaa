using TARpe22ShopVaitmaa.Core.Dto;
using TARpe22ShopVaitmaa.Core.ServiceInterface;

namespace TARpe22ShopVaitmaa.UnitTesting
{
    public class RealEstateTests : TestBase
    {
        [Fact]
        public async Task ShouldNot_AddEmptyRealEstate_WhenReturnResult()
        {
            //ülesseade
            RealEstateDto testDto = MockDataRealEstate();

            //läbiviimine
            var result = await Svc<IRealEstatesServices>().Create(testDto);

            //kontrollime
            Assert.NotNull(result);
        }


        //[Fact]
        //public async Task ShouldNot_GetByIdRealEstate_WhenReturnsIsNotEqual()
        //{
        //    // ülesseadistus
        //    Guid wrongGuid = Guid.NewGuid();
        //    Guid guid = Guid.Parse("c745c4a4-4ff1-4c3f-a722-024a905bca10");

        //    //teostus
        //    await Svc<IRealEstatesServices>().DetailAsync(guid); //gets one object

        //    //
        //    Assert.NotEqual(wrongGuid, guid);
        //}

        //[Fact]
        //public async Task Should_GetByIdRealEstate_WhenReturnsIsEqual()
        //{
        //    // ülesseadistus
        //    Guid existingGuid = Guid.Parse("c745c4a4-4ff1-4c3f-a722-024a905bca10");
        //    Guid guid = Guid.Parse("c745c4a4-4ff1-4c3f-a722-024a905bca10");

        //    //teostus
        //    await Svc<IRealEstatesServices>().DetailAsync(guid); //gets one object

        //    //
        //    Assert.Equal(existingGuid, guid);
        //}
        [Fact]
        public async Task Should_DeleteByIdRealestate_WhenDeleteRealEstate()
        {
            //ülesseade
            RealEstateDto dto = MockDataRealEstate();

            //tegevus
            var addRealEstate = await Svc<IRealEstatesServices>().Create(dto);
            addRealEstate.Id = Guid.NewGuid();
            var deleteRealEstate = await Svc<IRealEstatesServices>().Delete((Guid)addRealEstate.Id);

            //kontrollime
            Assert.Equal(addRealEstate.Id, deleteRealEstate.Id);
        }
        [Fact]
        public async Task ShouldNot_DeleteByIdRealEstate_DifferentRealEstate()
        {
            //ülesseade
            RealEstateDto dto = MockDataRealEstate();

            //tegevus
            var re1 = await Svc<IRealEstatesServices>().Create(dto);
            var re2 = await Svc<IRealEstatesServices>().Create(dto);

            var result = await Svc<IRealEstatesServices>().Delete((Guid)re2.Id);

            //kontroll
            Assert.NotEqual(re1.Id, result.Id);
        }

        [Fact]
        public async Task Should_UpdateRealEstate_OnUpdateMethodCall()
        {
            //ülesseade
            var guid = new Guid("12121212-2323-2323-2323-123412341234"); //8-4-4-4-12

            RealEstateDto dto = MockDataRealEstate();
            RealEstateDto domain = new();

            domain.Id = guid;
            domain.Address = "UPDATEDADDRESS";
            domain.County = "UPDATEDADDRESS";
            domain.Country = "UPDATEDADDRESS";
            domain.City = "UPDATEDADDRESS";
            domain.ModifiedAt = DateTime.UtcNow;
            //tegevus

            await Svc<IRealEstatesServices>().Update(dto);

            //kontroll
            Assert.Equal(domain.Id, guid);
            Assert.NotEqual(dto.Address, domain.Address);
            Assert.NotEqual(dto.County, domain.County);
            Assert.NotEqual(dto.Country, domain.Country);
            Assert.NotEqual(dto.City, domain.City);
            Assert.DoesNotMatch(dto.ModifiedAt.ToString(), domain.ModifiedAt.ToString());

        }
        [Fact]
        public async Task ShouldNot_UpdateRealEstate_WhenDataNull()
        {
            //ülesseade
            RealEstateDto existingdata = MockDataRealEstate();
            var createRealEstate = await Svc<IRealEstatesServices>().Create(existingdata);
            RealEstateDto nulldata = MockNullRealEstate();

            //tegevus
            var result = await Svc<IRealEstatesServices>().Update(nulldata);

            //kontroll
            Assert.NotEqual(createRealEstate.Id, result.Id);
        }
        [Fact]
        public async Task Should_CreateRealEstate_WhenAreaNegative()
        {
            //ülesseade
            RealEstateDto dto = new RealEstateDto
            {
                SquareMeters = -690,
                City = "Negative area test",
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
            };

            //tegevus
            var result = await Svc<IRealEstatesServices>().Create(dto);

            //kontrollimine
            Assert.NotNull(result);
            //Assert.Equal(result.SquareMeters, 690);
        }
        [Fact]
        public async Task ShouldUpdateModifiedAt_WhenUpdatingData()
        {
            //ülesseadistus
            RealEstateDto dto = MockDataRealEstate();
            var create = await Svc<IRealEstatesServices>().Create(dto);

            //tegevus
            RealEstateDto updated = MockDataRealEstate(true);
            var result = await Svc<IRealEstatesServices>().Update(updated);

            //kontroll
            Assert.NotEqual(create.ModifiedAt, result.ModifiedAt);
        }
        [Fact]
        public async Task ShouldNotUpdateCreatedAt_WhenUpdatingData()
        {
            //ülesseadistus
            RealEstateDto dto = MockDataRealEstate();
            var create = await Svc<IRealEstatesServices>().Create(dto);
            var originalTimestamp = "2027-11-17T09:17:22.9723933+02:00";

            //tegevus
            RealEstateDto update = MockDataRealEstate(true);
            var result = await Svc<IRealEstatesServices>().Update(update);
            result.CreatedAt = DateTime.Parse(originalTimestamp);

            //kontrollimine
            Assert.Equal(DateTime.Parse(originalTimestamp),result.CreatedAt);

        }
        [Fact]
        public async Task ShouldHaveUniqueID_WhenRealEstateCreated()
        {
            //ülesseadistus
            RealEstateDto dto1 = MockDataRealEstate();
            RealEstateDto dto2 = MockDataRealEstate();

            //tegevus
            var create1 = await Svc<IRealEstatesServices>().Create(dto1);
            var create2 = await Svc<IRealEstatesServices>().Create(dto2);

            //kontrollimine
            Assert.NotEqual(create1.Id, create2.Id);
        }
        [Fact]
        public async Task ShouldNotAdd_NullRealEstate()
        {
            //ülesseade
            RealEstateDto dto = MockDataRealEstate(true);

            //tegevus
            var result = await Svc<IRealEstatesServices>().Create(dto);

            //kontrollime
            Assert.NotNull(result);

        }
        [Fact]
        public async Task ShouldUpdate_WithCorrectDataType()
        {
            //ülesseade
            RealEstateDto dto = MockDataRealEstate();
            dto.ContactPhone = 236547;

            //tegevus
            var result = await Svc<IRealEstatesServices>().Create(dto);

            //kontrollimine
            Assert.IsType<int>(result.ContactPhone);
        }



        private static RealEstateDto MockDataRealEstate(bool isitupdating = false)
        {
            if (isitupdating == true)
            {
                return new()
                {
                    Type = Core.Domain.EstateType.Garage,
                    ListingDescription = "XXXXX",
                    Address = "YYYYY",
                    Country = "ZZZZZ",
                    County = "ÕÕÕÕÕ",
                    City = "ÄÄÄÄÄ",
                    PostalCode = 400,
                    ContactPhone = 500,
                    ContactFax = 600,
                    SquareMeters = 700,
                    Floor = 800,
                    FloorCount = 900,
                    Price = 1000,
                    RoomCount = 1100,
                    BathroomCount = 1200,
                    BedroomCount = 1300,
                    WhenEstateListedAt = DateTime.UtcNow.AddYears(1),
                    IsPropertySold = true,
                    DoesHaveSwimmingPool = true,
                    BuiltAt = DateTime.UtcNow.AddYears(1),
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow.AddYears(1),
                };
            }
            else
            {
                return new()
                {
                    Type = Core.Domain.EstateType.Apartment,
                    ListingDescription = "a",
                    Address = "b",
                    Country = "c",
                    County = "d",
                    City = "e",
                    PostalCode = 1,
                    ContactPhone = 2,
                    ContactFax = 3,
                    SquareMeters = 4,
                    Floor = 5,
                    FloorCount = 6,
                    Price = 7,
                    RoomCount = 8,
                    BathroomCount = 9,
                    BedroomCount = 10,
                    WhenEstateListedAt = DateTime.UtcNow,
                    IsPropertySold = false,
                    DoesHaveSwimmingPool = false,
                    BuiltAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                };
            }
        }

        private static RealEstateDto MockNullRealEstate()
        {
            return new()
            {
                Type = null,
                ListingDescription = null,
                Address = null,
                Country = null,
                County = null,
                City = null,
                PostalCode = null,
                ContactPhone = null,
                ContactFax = null,
                SquareMeters = null,
                Floor = null,
                FloorCount = null,
                Price = null,
                RoomCount = null,
                BathroomCount = null,
                BedroomCount = null,
                WhenEstateListedAt = null,
                IsPropertySold = null,
                DoesHaveSwimmingPool = null,
                BuiltAt = null,
                CreatedAt = null,
                ModifiedAt = null,
            };
        }
    }
}