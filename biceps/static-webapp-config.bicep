param stapp_name string
param app_config_name string

resource stapp 'Microsoft.Web/staticSites@2022-09-01' existing = {
  name: stapp_name
}

resource appConfig 'Microsoft.AppConfiguration/configurationStores@2023-03-01' existing = {
  name: app_config_name
}

resource settings 'Microsoft.Web/staticSites/config@2022-09-01' = {
  parent: stapp
  name: 'appsettings'
  properties: {
    connectionString: listKeys(appConfig.id, '2019-11-01-preview').value[0].connectionString
  }
}
