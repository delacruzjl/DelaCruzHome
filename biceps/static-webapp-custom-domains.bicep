param stapp_name string
param custom_domain_name string

resource stapp 'Microsoft.Web/staticSites@2021-02-01' existing = {
  name: stapp_name
}

resource custom_domain 'Microsoft.Web/staticSites/customDomains@2021-02-01' = if (!empty(custom_domain_name)) {
  parent: stapp
  name: custom_domain_name
}

resource www_domain 'Microsoft.Web/staticSites/customDomains@2021-02-01' = if (!empty(custom_domain_name)) {
  parent: stapp
  name: 'www.${custom_domain_name}'
}
