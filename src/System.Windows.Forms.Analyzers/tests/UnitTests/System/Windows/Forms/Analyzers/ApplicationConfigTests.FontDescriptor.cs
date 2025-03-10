﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using Xunit;
using Xunit.Abstractions;
using static System.Windows.Forms.Analyzers.ApplicationConfig;

namespace System.Windows.Forms.Analyzers.Tests
{
    public partial class ApplicationConfigTests
    {
        public class FontDescriptorTests
        {
            private readonly ITestOutputHelper _output;

            public FontDescriptorTests(ITestOutputHelper output)
            {
                _output = output;
            }

            [Fact]
            public void FontDescriptor_ctor()
            {
                FontDescriptor descriptor = new("fontName", 10f, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point);

                Assert.Equal("fontName", descriptor.Name);
                Assert.Equal(10f, descriptor.Size);
                Assert.Equal(FontStyle.Bold | FontStyle.Italic, descriptor.Style);
                Assert.Equal(GraphicsUnit.Point, descriptor.Unit);
            }

            [Theory]
            [InlineData("", "new Font(Control.DefaultFont.FontFamily, 10f, (FontStyle)3, (GraphicsUnit)3)")]
            [InlineData(" ", "new Font(Control.DefaultFont.FontFamily, 10f, (FontStyle)3, (GraphicsUnit)3)")]
            [InlineData("\t", "new Font(Control.DefaultFont.FontFamily, 10f, (FontStyle)3, (GraphicsUnit)3)")]
            [InlineData("fontName", "new Font(new FontFamily(\"fontName\"), 10f, (FontStyle)3, (GraphicsUnit)3)")]
            [InlineData("\"fontName\"", "new Font(new FontFamily(\"fontName\"), 10f, (FontStyle)3, (GraphicsUnit)3)")]
            [InlineData("Name with \tspaces", "new Font(new FontFamily(\"Name with spaces\"), 10f, (FontStyle)3, (GraphicsUnit)3)")]
            [InlineData("Name with 'quotes'", "new Font(new FontFamily(\"Name with quotes\"), 10f, (FontStyle)3, (GraphicsUnit)3)")]
            [InlineData("Name with \r\n lines", "new Font(new FontFamily(\"Name with  lines\"), 10f, (FontStyle)3, (GraphicsUnit)3)")]
            public void FontDescriptor_ToString(string fontName, string expected)
            {
                FontDescriptor descriptor = new(fontName, 10f, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point);

                _output.WriteLine(descriptor.ToString());
                Assert.Equal(expected, descriptor.ToString());
            }

            [Theory]
            [InlineData("ar-SA")]
            [InlineData("en-US")]
            [InlineData("es-ES")]
            [InlineData("fr-FR")]
            [InlineData("hi-IN")]
            [InlineData("ja-JP")]
            [InlineData("ru-RU")]
            [InlineData("tr-TR")]
            [InlineData("zh-CN")]
            public void FontDescriptor_ToString_culture_agnostic(string cultureName)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);

                FontDescriptor descriptor = new("Microsoft Sans Serif", 8.25f, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point);

                _output.WriteLine(descriptor.ToString());
                Assert.Equal("new Font(new FontFamily(\"Microsoft Sans Serif\"), 8.25f, (FontStyle)3, (GraphicsUnit)3)", descriptor.ToString());
            }
        }
    }
}
