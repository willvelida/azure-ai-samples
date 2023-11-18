@description('The suffix that will be applied to all resources in the template.')
param applicationSuffix string = uniqueString(resourceGroup().id)

@description('The name of the Azure AI Language service that will be deployed')
param azureAiServiceName string = 'ailang-${applicationSuffix}'

@description('The location that resources will be deployed to. Default is the location of the resource group')
param location string = resourceGroup().location

@description('The SKU that wil be applied to the Azure AI Service. Default is S0')
@allowed([
  'F0'
  'S0'
])
param sku string = 'S0'

resource cognitiveService 'Microsoft.CognitiveServices/accounts@2021-10-01' = {
  name: azureAiServiceName
  location: location
  sku: {
    name: sku
  }
  kind: 'CognitiveServices'
  properties: {
    apiProperties: {
      statisticsEnabled: false
    }
  }
}
