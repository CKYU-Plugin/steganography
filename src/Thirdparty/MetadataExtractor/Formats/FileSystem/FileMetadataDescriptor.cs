#region License
//
// Copyright 2002-2017 Drew Noakes
// Ported from Java to C# by Yakov Danilov for Imazen LLC in 2014
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
// More information about this project is available at:
//
//    https://github.com/drewnoakes/metadata-extractor-dotnet
//    https://drewnoakes.com/code/exif/
//
#endregion

using System.Diagnostics.CodeAnalysis;


namespace MetadataExtractor.Formats.FileSystem
{
    /// <author>Drew Noakes https://drewnoakes.com</author>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class FileMetadataDescriptor : TagDescriptor<FileMetadataDirectory>
    {
        public FileMetadataDescriptor( FileMetadataDirectory directory)
            : base(directory)
        {
        }

        public override string GetDescription(int tagType)
        {
            switch (tagType)
            {
                case FileMetadataDirectory.TagFileSize:
                    return GetFileSizeDescription();
                default:
                    return base.GetDescription(tagType);
            }
        }

        
        private string GetFileSizeDescription()
        {
            return Directory.TryGetInt64(FileMetadataDirectory.TagFileSize, out long size)
                ? size + " bytes"
                : null;
        }
    }
}
