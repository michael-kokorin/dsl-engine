namespace Modules.Core.Services.UI.Renderers
{
    using System;

    internal interface IDataRenderer<in TS, out T>
    {
        Func<TS, T> GetSpec();
    }
}