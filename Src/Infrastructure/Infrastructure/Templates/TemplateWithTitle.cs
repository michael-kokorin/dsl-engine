namespace Infrastructure.Templates
{
    using System;
    using System.Collections.Generic;

    using Common.Logging;

    internal sealed class TemplateWithTitle : ITemplateWithTitle
    {
        public ITemplate Title { get; }

        public ITemplate Body { get; }

        public TemplateWithTitle(ITemplate titleTemplate, ITemplate bodyTemplate)
        {
            if (titleTemplate == null) throw new ArgumentNullException(nameof(titleTemplate));
            if (bodyTemplate == null) throw new ArgumentNullException(nameof(bodyTemplate));

            Title = titleTemplate;
            Body = bodyTemplate;
        }

        [LogMethod]
        public void Add(IDictionary<string, object> properties)
        {
            Title.Add(properties);

            Body.Add(properties);
        }
    }
}