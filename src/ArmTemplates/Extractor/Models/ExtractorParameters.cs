using System;
using System.Collections.Generic;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Commands.Configurations;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Common.Extensions;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Common.FileHandlers;

namespace Microsoft.Azure.Management.ApiManagement.ArmTemplates.Extractor.Models
{
    public record ExtractorParameters
    {
        public string SourceApimName { get; private set; }

        public string DestinationApimName { get; private set; }

        public string ResourceGroup { get; private set; }

        /// <summary>
        /// Naming of all templates from the base folder requested by end-user in extractor-configuration parameters
        /// </summary>
        public FileNames FileNames { get; private set; }

        /// <summary>
        /// The root directory, where templates will be generated.
        /// </summary>
        public string FilesGenerationRootDirectory { get; private set; }

        /// <summary>
        /// Name of a single API that user wants to extract
        /// </summary>
        public string SingleApiName { get; private set; }

        /// <summary>
        /// Names of APIs that user wants to extract
        /// </summary>
        public List<string> MultipleApiNames { get; private set; }

        /// <summary>
        /// Create separate api templates for every API in the source APIM instance
        /// </summary>
        public bool SplitApis { get; private set; }

        /// <summary>
        /// Are revisions included to templates
        /// </summary>
        public bool IncludeAllRevisions { get; private set; }

        public string LinkedTemplatesBaseUrl { get; private set; }

        public string LinkedTemplatesSasToken { get; private set; }

        public string LinkedTemplatesUrlQueryString { get; private set; }

        public string PolicyXMLBaseUrl { get; private set; }

        public string PolicyXMLSasToken { get; private set; }

        public string ApiVersionSetName { get; private set; }

        public ServiceUrlProperty[] ServiceUrlParameters { get; private set; }

        public bool ParameterizeServiceUrl { get; private set; }

        public bool ParameterizeNamedValue { get; private set; }

        public bool ParameterizeApiLoggerId { get; private set; }

        public bool ParameterizeLogResourceId { get; private set; }

        public bool NotIncludeNamedValue { get; private set; }

        public bool ParamNamedValuesKeyVaultSecrets { get; private set; }

        public int OperationBatchSize { get; private set; }

        public bool ParameterizeBackend { get; private set; }

        public bool ExtractGateways { get; set; }

        public ExtractorParameters(ExtractorConsoleAppConfiguration extractorConfig)
        {
            this.SourceApimName = extractorConfig.SourceApimName;
            this.DestinationApimName = extractorConfig.DestinationApimName;
            this.ResourceGroup = extractorConfig.ResourceGroup;
            this.FilesGenerationRootDirectory = extractorConfig.FileFolder;
            this.SingleApiName = extractorConfig.ApiName;
            this.LinkedTemplatesBaseUrl = extractorConfig.LinkedTemplatesBaseUrl;
            this.LinkedTemplatesSasToken = extractorConfig.LinkedTemplatesSasToken;
            this.LinkedTemplatesUrlQueryString = extractorConfig.LinkedTemplatesUrlQueryString;
            this.PolicyXMLBaseUrl = extractorConfig.PolicyXMLBaseUrl;
            this.PolicyXMLSasToken = extractorConfig.PolicyXMLSasToken;
            this.ApiVersionSetName = extractorConfig.ApiVersionSetName;
            this.IncludeAllRevisions = "true".Equals(extractorConfig.IncludeAllRevisions, StringComparison.OrdinalIgnoreCase);
            this.ServiceUrlParameters = extractorConfig.ServiceUrlParameters;
            this.ParameterizeServiceUrl = "true".Equals(extractorConfig.ParamServiceUrl, StringComparison.OrdinalIgnoreCase) || extractorConfig.ServiceUrlParameters != null;
            this.ParameterizeNamedValue = "true".Equals(extractorConfig.ParamNamedValue, StringComparison.OrdinalIgnoreCase);
            this.ParameterizeApiLoggerId = "true".Equals(extractorConfig.ParamApiLoggerId, StringComparison.OrdinalIgnoreCase);
            this.ParameterizeLogResourceId = "true".Equals(extractorConfig.ParamLogResourceId, StringComparison.OrdinalIgnoreCase);
            this.NotIncludeNamedValue = "true".Equals(extractorConfig.NotIncludeNamedValue, StringComparison.OrdinalIgnoreCase);
            this.OperationBatchSize = extractorConfig.OperationBatchSize ?? default;
            this.ParamNamedValuesKeyVaultSecrets = "true".Equals(extractorConfig.ParamNamedValuesKeyVaultSecrets, StringComparison.OrdinalIgnoreCase);
            this.ParameterizeBackend = "true".Equals(extractorConfig.ParamBackend, StringComparison.OrdinalIgnoreCase);
            this.SplitApis = "true".Equals(extractorConfig.SplitAPIs, StringComparison.OrdinalIgnoreCase);
            this.IncludeAllRevisions = "true".Equals(extractorConfig.IncludeAllRevisions, StringComparison.OrdinalIgnoreCase);
            this.FileNames = this.GenerateFileNames(extractorConfig.BaseFileName, extractorConfig.SourceApimName);
            this.MultipleApiNames = this.ParseMultipleApiNames(extractorConfig.MultipleAPIs);
            this.ExtractGateways = "true".Equals(extractorConfig.ExtractGateways, StringComparison.OrdinalIgnoreCase);
        }

        internal ExtractorParameters OverrideConfiguration(ExtractorConsoleAppConfiguration overridingConfig)
        {
            if (overridingConfig == null) return this;

            this.SourceApimName = overridingConfig.SourceApimName ?? this.SourceApimName;
            this.DestinationApimName = overridingConfig.DestinationApimName ?? this.DestinationApimName;
            this.ResourceGroup = overridingConfig.ResourceGroup ?? this.ResourceGroup;
            this.FilesGenerationRootDirectory = overridingConfig.FileFolder ?? this.FilesGenerationRootDirectory;
            this.SingleApiName = overridingConfig.ApiName ?? this.SingleApiName;
            this.LinkedTemplatesBaseUrl = overridingConfig.LinkedTemplatesBaseUrl ?? this.LinkedTemplatesBaseUrl;
            this.LinkedTemplatesSasToken = overridingConfig.LinkedTemplatesSasToken ?? this.LinkedTemplatesSasToken;
            this.LinkedTemplatesUrlQueryString = overridingConfig.LinkedTemplatesUrlQueryString ?? this.LinkedTemplatesUrlQueryString;
            this.PolicyXMLBaseUrl = overridingConfig.PolicyXMLBaseUrl ?? this.PolicyXMLBaseUrl;
            this.PolicyXMLSasToken = overridingConfig.PolicyXMLSasToken ?? this.PolicyXMLSasToken;
            this.ApiVersionSetName = overridingConfig.ApiVersionSetName ?? this.ApiVersionSetName;
            this.IncludeAllRevisions = this.OverrideBoolParam(this.IncludeAllRevisions, overridingConfig.IncludeAllRevisions);

            // there can be no service url parameters in overriding configuration
            // this.ServiceUrlParameters = overridingConfig.ServiceUrlParameters ?? this.ServiceUrlParameters;

            this.ParameterizeServiceUrl = this.OverrideBoolParam(this.ParameterizeServiceUrl, overridingConfig.ParamServiceUrl);
            this.ParameterizeNamedValue = this.OverrideBoolParam(this.ParameterizeNamedValue, overridingConfig.ParamNamedValue);
            this.ParameterizeApiLoggerId = this.OverrideBoolParam(this.ParameterizeApiLoggerId, overridingConfig.ParamApiLoggerId);
            this.ParameterizeLogResourceId = this.OverrideBoolParam(this.ParameterizeLogResourceId, overridingConfig.ParamLogResourceId);
            this.NotIncludeNamedValue = this.OverrideBoolParam(this.NotIncludeNamedValue, overridingConfig.NotIncludeNamedValue);
            this.OperationBatchSize = overridingConfig.OperationBatchSize ?? this.OperationBatchSize;
            this.ParamNamedValuesKeyVaultSecrets = this.OverrideBoolParam(this.ParamNamedValuesKeyVaultSecrets, overridingConfig.ParamNamedValuesKeyVaultSecrets);
            this.ParameterizeBackend = this.OverrideBoolParam(this.ParameterizeBackend, overridingConfig.ParamBackend);
            this.SplitApis = this.OverrideBoolParam(this.SplitApis, overridingConfig.SplitAPIs);
            this.IncludeAllRevisions = this.OverrideBoolParam(this.IncludeAllRevisions, overridingConfig.IncludeAllRevisions);
            this.ExtractGateways = this.OverrideBoolParam(this.ExtractGateways, overridingConfig.ExtractGateways);

            if (!string.IsNullOrEmpty(overridingConfig.BaseFileName) && !string.IsNullOrEmpty(overridingConfig.SourceApimName))
            {
                this.FileNames = this.GenerateFileNames(overridingConfig.BaseFileName, overridingConfig.SourceApimName);
            }

            if (!string.IsNullOrEmpty(overridingConfig.MultipleAPIs))
            {
                this.MultipleApiNames = this.ParseMultipleApiNames(overridingConfig.MultipleAPIs);
            }

            return this;
        }

        bool OverrideBoolParam(bool paramValue, string overrideValue)
        {
            if(string.IsNullOrEmpty(overrideValue))
            {
                return paramValue;
            }
            else
            {
                return overrideValue.Equals("true", StringComparison.OrdinalIgnoreCase);
            }
        }

        List<string> ParseMultipleApiNames(string multipleApisArgument)
        {
            if (string.IsNullOrEmpty(multipleApisArgument)) return null;            

            var validApiNames = new List<string>();
            foreach (var apiName in multipleApisArgument.Split(','))
            {
                var trimmedApiName = apiName.Trim();
                if (!string.IsNullOrEmpty(trimmedApiName))
                {
                    validApiNames.Add(trimmedApiName);
                }
            }

            return validApiNames;
        }

        FileNames GenerateFileNames(string userRequestedGenerationBaseFolder, string sourceApimInstanceName)
            => string.IsNullOrEmpty(userRequestedGenerationBaseFolder)  
                ? FileNameGenerator.GenerateFileNames(sourceApimInstanceName) 
                : FileNameGenerator.GenerateFileNames(userRequestedGenerationBaseFolder);

        public void Validate()
        {
            if (string.IsNullOrEmpty(this.SourceApimName)) throw new ArgumentException("Missing parameter <sourceApimName>.");
            if (string.IsNullOrEmpty(this.DestinationApimName)) throw new ArgumentException("Missing parameter <destinationApimName>.");
            if (string.IsNullOrEmpty(this.ResourceGroup)) throw new ArgumentException("Missing parameter <resourceGroup>.");
            if (string.IsNullOrEmpty(this.FilesGenerationRootDirectory)) throw new ArgumentException("Missing parameter <fileFolder>.");

            bool hasVersionSetName = !string.IsNullOrEmpty(this.ApiVersionSetName);
            bool hasSingleApi = this.SingleApiName != null;
            bool hasMultipleAPIs = !this.MultipleApiNames.IsNullOrEmpty();

            if (this.SplitApis && hasSingleApi)
            {
                throw new NotSupportedException("Can't use splitAPIs and apiName at same time");
            }

            if (this.SplitApis && hasVersionSetName)
            {
                throw new NotSupportedException("Can't use splitAPIs and apiVersionSetName at same time");
            }

            if ((hasVersionSetName || hasSingleApi) && hasMultipleAPIs)
            {
                throw new NotSupportedException("Can't use multipleAPIs with apiName or apiVersionSetName at the same time");
            }

            if (hasSingleApi && hasVersionSetName)
            {
                throw new NotSupportedException("Can't use apiName and apiVersionSetName at same time");
            }

            if (!hasSingleApi && this.IncludeAllRevisions)
            {
                throw new NotSupportedException("\"includeAllRevisions\" can be used when you specify the API you want to extract with \"apiName\"");
            }
        }
    }
}
