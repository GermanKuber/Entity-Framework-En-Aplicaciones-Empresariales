using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Moq;

namespace Market.Core
{
    //TODO 3 - Genero un metodo helper para mockear los DbSet
    public static class TestHelpers
    {
        public static DbSet<T> MockDbSet<T>() where T : class
        {
            return MockDbSet<T>(null);
        }

        public static DbSet<T> MockDbSet<T>(List<T> inMemoryData) where T : class
        {
            if (inMemoryData == null)
            {
                inMemoryData = new List<T>();
            }
            var mockDbSet = new Mock<DbSet<T>>();
            var queryableData = inMemoryData.AsQueryable();

            mockDbSet.Setup(m => m.Add(It.IsAny<T>())).Callback<T>(inMemoryData.Add);
            //TODO 9 - Comento el proovedor
            //mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryableData.Provider);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryableData.GetEnumerator());
            //TODO 5 - Mockeo Include y AsNoTracking
            mockDbSet.Setup(x => x.AsNoTracking()).Returns(mockDbSet.Object);
            mockDbSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockDbSet.Object);

            //TODO 8 - Registro en mi Mock los nuevos metodos async
            mockDbSet.As<IDbAsyncEnumerable<T>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<T>(queryableData.GetEnumerator()));

            mockDbSet.As<IQueryable<T>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<T>(queryableData.Provider));

            return mockDbSet.Object;
        }
    }
}