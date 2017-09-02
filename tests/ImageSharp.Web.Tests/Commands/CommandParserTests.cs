﻿namespace SixLabors.ImageSharp.Web.Tests.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using global::ImageSharp;
    using global::ImageSharp.Processing;
    using ImageSharp.Web.Commands;

    using Xunit;

    public class CommandParserTests
    {
        [Fact]
        public void CommandParsesIntegralValues()
        {
            string param = "1";

            sbyte sb = CommandParser.Instance.ParseValue<sbyte>(param);
            Assert.NotNull(sb);
            Assert.Equal((sbyte)1, sb);

            byte b = CommandParser.Instance.ParseValue<byte>(param);
            Assert.NotNull(b);
            Assert.Equal((byte)1, b);

            short s = CommandParser.Instance.ParseValue<short>(param);
            Assert.NotNull(s);
            Assert.Equal((short)1, s);

            ushort us = CommandParser.Instance.ParseValue<ushort>(param);
            Assert.NotNull(us);
            Assert.Equal((ushort)1, us);

            int i = CommandParser.Instance.ParseValue<int>(param);
            Assert.NotNull(i);
            Assert.Equal(1, i);

            uint ui = CommandParser.Instance.ParseValue<uint>(param);
            Assert.NotNull(ui);
            Assert.Equal(1u, ui);

            long l = CommandParser.Instance.ParseValue<long>(param);
            Assert.NotNull(l);
            Assert.Equal(1L, l);

            ulong ul = CommandParser.Instance.ParseValue<ulong>(param);
            Assert.NotNull(i);
            Assert.Equal(1uL, ul);

            float f = CommandParser.Instance.ParseValue<float>(param);
            Assert.NotNull(f);
            Assert.Equal(1F, f);

            double d = CommandParser.Instance.ParseValue<double>(param);
            Assert.NotNull(d);
            Assert.Equal(1D, d);

            decimal m = CommandParser.Instance.ParseValue<decimal>(param);
            Assert.NotNull(m);
            Assert.Equal(1M, m);
        }

        [Fact]
        public void CommandParsesRealValues()
        {
            // Math.PI.ToString() chops two digits off the end
            const double Pi = 3.14159265358979;
            string param = Pi.ToString(CultureInfo.InvariantCulture);
            double rounded = Math.Round(Pi, MidpointRounding.AwayFromZero);

            sbyte sb = CommandParser.Instance.ParseValue<sbyte>(param);
            Assert.NotNull(sb);
            Assert.Equal((sbyte)rounded, sb);

            byte b = CommandParser.Instance.ParseValue<byte>(param);
            Assert.NotNull(b);
            Assert.Equal((byte)rounded, b);

            short s = CommandParser.Instance.ParseValue<short>(param);
            Assert.NotNull(s);
            Assert.Equal((short)rounded, s);

            ushort us = CommandParser.Instance.ParseValue<ushort>(param);
            Assert.NotNull(us);
            Assert.Equal((ushort)rounded, us);

            int i = CommandParser.Instance.ParseValue<int>(param);
            Assert.NotNull(i);
            Assert.Equal((int)rounded, i);

            uint ui = CommandParser.Instance.ParseValue<uint>(param);
            Assert.NotNull(i);
            Assert.Equal((uint)rounded, ui);

            long l = CommandParser.Instance.ParseValue<long>(param);
            Assert.NotNull(l);
            Assert.Equal((long)rounded, l);

            ulong ul = CommandParser.Instance.ParseValue<ulong>(param);
            Assert.NotNull(i);
            Assert.Equal((ulong)rounded, ul);

            float f = CommandParser.Instance.ParseValue<float>(param);
            Assert.NotNull(f);
            Assert.Equal((float)Pi, f);

            double d = CommandParser.Instance.ParseValue<double>(param);
            Assert.NotNull(d);
            Assert.Equal(Pi, d);

            decimal m = CommandParser.Instance.ParseValue<decimal>(param);
            Assert.NotNull(m);
            Assert.Equal((decimal)Pi, m);
        }

        [Fact]
        public void CommandParsesEnums()
        {
            string param = "max";

            ResizeMode mode = CommandParser.Instance.ParseValue<ResizeMode>(param);

            Assert.NotNull(mode);
            Assert.Equal(ResizeMode.Max, mode);

            // Check default is returned.
            param = "hjbjhbjh";
            mode = CommandParser.Instance.ParseValue<ResizeMode>(param);

            Assert.NotNull(mode);
            Assert.Equal(ResizeMode.Crop, mode);
        }

        [Fact]
        public void CommandParsesIntegralArrays()
        {
            string param = "1,2,3,4";

            int[] actual = CommandParser.Instance.ParseValue<int[]>(param);

            Assert.NotNull(actual);
            Assert.Equal(new[] { 1, 2, 3, 4 }, actual);
        }

        [Fact]
        public void CommandParsesRealArrays()
        {
            string param = "1.667,2.667,3.667,4.667";

            float[] actual = CommandParser.Instance.ParseValue<float[]>(param);

            Assert.NotNull(actual);
            Assert.Equal(new[] { 1.667F, 2.667F, 3.667F, 4.667F }, actual);
        }

        [Fact]
        public void CommandParsesIntegralLists()
        {
            string param = "1,2,3,4";

            List<int> actual = CommandParser.Instance.ParseValue<List<int>>(param);

            Assert.NotNull(actual);
            Assert.Equal(new List<int> { 1, 2, 3, 4 }, actual);
        }

        [Fact]
        public void CommandParsesRealLists()
        {
            string param = "1.667,2.667,3.667,4.667";

            List<float> actual = CommandParser.Instance.ParseValue<List<float>>(param);

            Assert.NotNull(actual);
            Assert.Equal(new List<float> { 1.667F, 2.667F, 3.667F, 4.667F }, actual);
        }

        [Fact]
        public void CommandParsesRgba32()
        {
            string param = "255,255,255";
            Rgba32 actual = CommandParser.Instance.ParseValue<Rgba32>(param);

            Assert.NotNull(actual);
            Assert.Equal(Rgba32.White, actual);

            string param2 = "255,255,255,0";
            Rgba32 actual2 = CommandParser.Instance.ParseValue<Rgba32>(param2);

            Assert.NotNull(actual2);
            Assert.Equal(Rgba32.Transparent, actual2);

            string param3 = "orange";
            Rgba32 actual3 = CommandParser.Instance.ParseValue<Rgba32>(param3);

            Assert.NotNull(actual3);
            Assert.Equal(Rgba32.Orange, actual3);

            string param4 = "00FF00";
            Rgba32 actual4 = CommandParser.Instance.ParseValue<Rgba32>(param4);

            Assert.NotNull(actual4);
            Assert.Equal(Rgba32.Lime, actual4);
        }
    }
}