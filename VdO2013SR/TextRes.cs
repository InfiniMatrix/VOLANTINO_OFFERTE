using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VdO2013SRCore;

namespace VdO2013SR
{
    public class TextRes : VdO2013SRCore.TextRes
    {
        public static class EmptySiteReader
        {
            public const String KEmpty = "Empty";
            public const ReaderMode KMode = ReaderMode.Empty;
        }

        public const String KClassDoesNotImplementStaticMethodFmt = @"{0} does not implement static {1} method";

        public static class DefaultSettingsNames
        {
            public const String KNAME = "Name";
            public const String KCODE = "Code";
            public const String KNormalSiteUrl = "NormalSiteUrl";
            public const String KMobileSiteUrl = "MobileSiteUrl";

            public const string KFeaturesXPath = "FeaturesXPath";
            public const string KFeaturesEnabled = "FeaturesEnabled";

            public const String KAgenzia = "Agenzia";
            public const String KRagioneSociale = "RagioneSociale";
            public const String KTipologia = "Tipologia";
            public const String KElementi = "Elementi";
            public const String KPagine = "Pagine";
            public const String KReportPath = "ReportPath";
            public const String KImagesPath = "ImagesPath";
            public const String KDataPath = "DataPath";
            public const String KImageFormat = "ImageFormat";

            public const String KTimeStamp = "TimeStamp";
            public const String KReaderVersion = "ReaderVersion";
            public const String KVersion = "Version";
            public const String KSettingsVersion = "SettingsVersion";
            public const String KReleaseDate = "ReleaseDate";

            public const String KMergeDefinitionTypeName = @"VdO2013Data.OfferteData, VdO2013Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
            public const String KMergeDefinitionName = "MergeDefinitionName";
            public const String KMergeDefinitionKeyFields = "MergeDefinitionKeyFields";
            public const String KMergeDefinitionColExclude = "MergeDefinitionColExclude";
            public const String KMergeDefinitionRowExclude = "MergeDefinitionRowExclude";
        }// class DefaultSettingsNames
    }
}
