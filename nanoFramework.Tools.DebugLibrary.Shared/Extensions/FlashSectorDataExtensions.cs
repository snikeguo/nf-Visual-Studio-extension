//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.Tools.Debugger.WireProtocol;
using System.Collections.Generic;
using static nanoFramework.Tools.Debugger.WireProtocol.Commands.Monitor_FlashSectorMap;

namespace nanoFramework.Tools.Debugger.Extensions
{
    public static class FlashSectorDataExtensions
    {
        /// <summary>
        /// Convert a <see cref="FlashSectorData"/> into a <see cref="DeploymentSector"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DeploymentSector ToDeploymentSector(this FlashSectorData value)
        {
            // build a DeploymentSector from a FlashSectorData

            List<DeploymentBlock> blocks = new List<DeploymentBlock>();

            for (int i = 0; i < value.NumBlocks; i++)
            {
                int programmingAlignment = 0;

                // check alignment requirements
                var flags = value.Flags& BlockRegionAttributes_MASK;
                if (flags!=0)
                {
                    if ((flags & BlockRegionAttribute_ProgramWidthIs64bits) == BlockRegionAttribute_ProgramWidthIs64bits)
                    {
                        // programming width is 64bits => 8 bytes
                        programmingAlignment = 8;
                    }
                    else if ((flags & BlockRegionAttribute_ProgramWidthIs32bits) == BlockRegionAttribute_ProgramWidthIs32bits)
                    {
                        programmingAlignment = 4;
                    }
                    else if ((flags & BlockRegionAttribute_ProgramWidthIs128bits) == BlockRegionAttribute_ProgramWidthIs128bits)
                    {
                        programmingAlignment = 16;
                    }
                    else if ((flags & BlockRegionAttribute_ProgramWidthIs256bits) == BlockRegionAttribute_ProgramWidthIs256bits)
                    {
                        programmingAlignment = 32;
                    }
                    else if ((flags & BlockRegionAttribute_ProgramWidthIs512bits) == BlockRegionAttribute_ProgramWidthIs512bits)
                    {
                        programmingAlignment = 64;
                    }
                    else if ((flags & BlockRegionAttribute_ProgramWidthIs1024bits) == BlockRegionAttribute_ProgramWidthIs1024bits)
                    {
                        programmingAlignment = 128;
                    }
                    else if ((flags & BlockRegionAttribute_ProgramWidthIs2048bits) == BlockRegionAttribute_ProgramWidthIs2048bits)
                    {
                        programmingAlignment = 256;
                    }
                    else if ((flags & BlockRegionAttribute_ProgramWidthIs4096bits) == BlockRegionAttribute_ProgramWidthIs4096bits)
                    {
                        programmingAlignment = 512;
                    }
                    else if ((flags & BlockRegionAttribute_ProgramWidthIs8192bits) == BlockRegionAttribute_ProgramWidthIs8192bits)
                    {
                        programmingAlignment = 1024;
                    }
                    else if ((flags & BlockRegionAttribute_ProgramWidthIs16384bits) == BlockRegionAttribute_ProgramWidthIs16384bits)
                    {
                        programmingAlignment = 2048;
                    }
                    else if ((flags & BlockRegionAttribute_ProgramWidthIs32768bits) == BlockRegionAttribute_ProgramWidthIs32768bits)
                    {
                        programmingAlignment = 4096;
                    }
                    else
                    {
                        programmingAlignment = 0;
                    }
                }

                blocks.Add(new DeploymentBlock(
                    (int)value.StartAddress + (i * (int)value.BytesPerBlock),
                    (int)value.BytesPerBlock,
                    programmingAlignment));
            }

            return new DeploymentSector(blocks);
        }
    }
}
