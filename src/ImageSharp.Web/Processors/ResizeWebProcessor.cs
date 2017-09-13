﻿// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp.Web.Commands;
using SixLabors.Primitives;

namespace SixLabors.ImageSharp.Web.Processors
{
    /// <summary>
    /// Allows the resizing of images.
    /// </summary>
    public class ResizeWebProcessor : IImageWebProcessor
    {
        /// <summary>
        /// The command constant for the resize width
        /// </summary>
        public const string Width = "width";

        /// <summary>
        /// The command constant for the resize height
        /// </summary>
        public const string Height = "height";

        /// <summary>
        /// The command constant for the resize focal point coordinates
        /// </summary>
        public const string Xy = "rxy";

        /// <summary>
        /// The command constant for the resize mode
        /// </summary>
        public const string Mode = "rmode";

        /// <summary>
        /// The command constant for the resize sampler
        /// </summary>
        public const string Sampler = "rsampler";

        /// <summary>
        /// The command constant for the resize sampler
        /// </summary>
        public const string Anchor = "ranchor";

        /// <summary>
        /// The command constant for the resize compand mode
        /// </summary>
        public const string Compand = "compand";

        private static readonly IEnumerable<string> ResizeCommands
            = new[]
            {
                Width,
                Height,
                Xy,
                Mode,
                Sampler,
                Anchor,
                Compand
            };

        /// <inheritdoc/>
        public IEnumerable<string> Commands { get; } = ResizeCommands;

        /// <inheritdoc/>
        public FormattedImage Process(FormattedImage image, ILogger logger, IDictionary<string, string> commands)
        {
            ResizeOptions options = GetResizeOptions(commands);

            if (options != null)
            {
                image.Image.Mutate(x => x.Resize(options));
            }

            return image;
        }

        private static ResizeOptions GetResizeOptions(IDictionary<string, string> commands)
        {
            if (!commands.ContainsKey(Width) && !commands.ContainsKey(Height))
            {
                return null;
            }

            CommandParser parser = CommandParser.Instance;
            Size size = ParseSize(commands, parser);

            if (size.Width <= 0 && size.Height <= 0)
            {
                return null;
            }

            var options = new ResizeOptions
            {
                Size = size,
                CenterCoordinates = GetCenter(commands, parser),
                Position = GetAnchor(commands, parser),
                Mode = GetMode(commands, parser),
                Compand = GetCompandMode(commands, parser),
            };

            // Defaults to Bicubic if not set.
            IResampler sampler = GetSampler(commands);
            if (sampler != null)
            {
                options.Sampler = sampler;
            }

            return options;
        }

        private static Size ParseSize(IDictionary<string, string> commands, CommandParser parser)
        {
            // The command parser will reject negative numbers as it clamps values to ranges.
            uint width = parser.ParseValue<uint>(commands.GetValueOrDefault(Width));
            uint height = parser.ParseValue<uint>(commands.GetValueOrDefault(Height));

            return new Size((int)width, (int)height);
        }

        private static float[] GetCenter(IDictionary<string, string> commands, CommandParser parser)
        {
            return parser.ParseValue<float[]>(commands.GetValueOrDefault(Xy));
        }

        private static ResizeMode GetMode(IDictionary<string, string> commands, CommandParser parser)
        {
            return parser.ParseValue<ResizeMode>(commands.GetValueOrDefault(Mode));
        }

        private static AnchorPosition GetAnchor(IDictionary<string, string> commands, CommandParser parser)
        {
            return parser.ParseValue<AnchorPosition>(commands.GetValueOrDefault(Anchor));
        }

        private static bool GetCompandMode(IDictionary<string, string> commands, CommandParser parser)
        {
            return parser.ParseValue<bool>(commands.GetValueOrDefault(Compand));
        }

        private static IResampler GetSampler(IDictionary<string, string> commands)
        {
            string sampler = commands.GetValueOrDefault(Sampler);

            if (sampler != null)
            {
                switch (sampler.ToLowerInvariant())
                {
                    case "nearest": return new NearestNeighborResampler();
                    case "box": return new BoxResampler();
                    case "mitchell": return new MitchellNetravaliResampler();
                    case "catmull": return new CatmullRomResampler();
                    case "lanczos2": return new Lanczos2Resampler();
                    case "lanczos3": return new Lanczos3Resampler();
                    case "lanczos5": return new Lanczos5Resampler();
                    case "lanczos8": return new Lanczos8Resampler();
                    case "welch": return new WelchResampler();
                    case "triangle": return new TriangleResampler();
                    case "hermite": return new HermiteResampler();
                }
            }

            return null;
        }
    }
}