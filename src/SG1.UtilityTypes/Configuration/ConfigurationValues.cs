using Microsoft.CodeAnalysis;
using System;

namespace SG1.UtilityTypes.Configuration
{
    internal sealed class ConfigurationValues
    {
        private const string IndentSizeKey = "indent_size";
        private const uint IndentSizeDefaultValue = 4u;
        private const string IndentStyleKey = "indent_style";
        private const IndentStyle IndentStyleDefaultValue = IndentStyle.Space;

        public ConfigurationValues(GeneratorExecutionContext context, SyntaxTree tree)
        {
            var options = context.AnalyzerConfigOptions.GetOptions(tree);

            this.IndentStyle = options.TryGetValue(ConfigurationValues.IndentStyleKey, out var indentStyle) ?
                (Enum.TryParse<IndentStyle>(indentStyle, out var indentStyleValue) ? indentStyleValue : ConfigurationValues.IndentStyleDefaultValue) :
                ConfigurationValues.IndentStyleDefaultValue;
            this.IndentSize = options.TryGetValue(ConfigurationValues.IndentSizeKey, out var indentSize) ?
                (uint.TryParse(indentSize, out var indentSizeValue) ? indentSizeValue : ConfigurationValues.IndentSizeDefaultValue) :
                ConfigurationValues.IndentSizeDefaultValue;
        }

        internal IndentStyle IndentStyle { get; }
        internal uint IndentSize { get; }
    }
}