param configName string
param itemLabel string = ''
param itemKey string
param itemValue string
param itemContentType string = ''
param itemTag object = {}

resource appcs 'Microsoft.AppConfiguration/configurationStores@2021-03-01-preview' existing = {
  name: configName
}

resource appConfigKeys 'Microsoft.AppConfiguration/configurationStores/keyValues@2020-07-01-preview' = {
  parent: appcs
  name: (empty(itemLabel) ? '${itemKey}$${itemLabel}' : itemKey)
  properties: {
    value: itemValue
    contentType: (empty(itemContentType) ? itemContentType : null)
    tags: (empty(itemTag) ? itemTag : null)
  }
}
