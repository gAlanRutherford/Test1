using Autofac;
using Autofac.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using Moq;
using AnsibleAPI.Domain.Abstract;
using AnsibleAPI.Domain.Entities;

namespace AnsibleAPI.App_Start
{
    public class AutoFacWebapiConfig
    {
        public static IContainer Container;

        public static void Initialize(HttpConfiguration config)
        {
            Initialize(config, RegisterServices(new ContainerBuilder()));
        }

        private void MockRepository()
        {

        }

        public static void Initialize(HttpConfiguration config, IContainer container)
        {
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static void MockRepository(ContainerBuilder builder)
        {
            var mockEntityRepository = new Mock<IEntityRepository>();
            var lastId = 4;
            var mockEntities = new List<Entity>
            {
                new Entity(1, "Test Name 1"),
                new Entity(2, "Test Name 2"),
                new Entity(3, "Test Name 3"),
                new Entity(lastId, "Test Name 4"),
            };
            mockEntityRepository.Setup(m => m.GetEntities()).Returns(mockEntities);
            mockEntityRepository.Setup(m => m.GetEntiy(It.IsAny<int>()))
                .Returns<int>(id =>
                {
                    return mockEntities.FirstOrDefault(y => y.Id == id);
                });

            mockEntityRepository.Setup(m => m.AddEntity(It.IsAny<Entity>()))
                .Callback<Entity>(e =>
                {
                    lastId++;
                    e.Id = lastId;
                    mockEntities.Add(e);
                })
                .Returns<Entity>(x =>
                {
                    return x.Id.Value;
                });

            mockEntityRepository.Setup(m => m.AddEntities(It.IsAny<IEnumerable<Entity>>()))
                .Callback<IEnumerable<Entity>>(entityList =>
                {
                    foreach (var entity in entityList)
                    {
                        lastId++;
                        entity.Id = lastId;
                        mockEntities.Add(entity);
                    }
                })
                .Returns<IEnumerable<Entity>>(entityList =>
                {
                    return entityList.Select(x => x.Id.Value);
                });
            builder.RegisterInstance(mockEntityRepository.Object).As<IEntityRepository>();
        }

        private static IContainer RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<IEntityRepository>();
            MockRepository(builder);
            Container = builder.Build();

            return Container;
        }
    }
}