@allowed([
  'brazilsouth'
  'canadacentral'
  'centralus'
  'eastus'
  'eastus2'
  'southcentralus'
  'usgovvirginia'
  'westus2'
  'westus3 '
])
param location string
param name string

resource appcs 'Microsoft.AppConfiguration/configurationStores@2021-03-01-preview' = {
  name: name
  location: location
  sku: {
    name: 'free'
  }
  properties: {
    encryption: {}
    disableLocalAuth: false

  }
}

output id string = appcs.id

