using System;
using System.Collections.Generic;
using System.Text;
using Autofac;

namespace Server.Core.IoC
{
    public static class UsefulContainer
    {
        public static readonly ContainerBuilder Builder = new ContainerBuilder();

        public static IContainer Instance { get; private set; }

        public static void Initialize()
        {
            Instance = Builder.Build();
        }
    }
}
