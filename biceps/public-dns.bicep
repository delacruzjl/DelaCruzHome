param custom_domain_name string
param stapp_name string

resource stapp 'Microsoft.Web/staticSites@2021-02-01' existing = {
  name: stapp_name
}

var unique_txt = '7qvtvrc3nht5rmx8l45zx1n4h1mkzt43'

resource public_dns 'Microsoft.Network/dnszones@2018-05-01' = {
  name: custom_domain_name
  location: 'global'
  properties: {
    zoneType: 'Public'
  }
}

resource root_dns 'Microsoft.Network/dnszones/NS@2018-05-01' = {
  parent: public_dns
  name: '@'
  properties: {
    TTL: 172800
    NSRecords: [
      {
        nsdname: 'ns1-09.azure-dns.com.'
      }
      {
        nsdname: 'ns2-09.azure-dns.net.'
      }
      {
        nsdname: 'ns3-09.azure-dns.org.'
      }
      {
        nsdname: 'ns4-09.azure-dns.info.'
      }
    ]
  }
}

resource soa_dns 'Microsoft.Network/dnszones/SOA@2018-05-01' = {
  parent: public_dns
  name: '@'
  properties: {
    TTL: 3600
    SOARecord: {
      email: 'azuredns-hostmaster.microsoft.com'
      expireTime: 2419200
      host: 'ns1-09.azure-dns.com.'
      minimumTTL: 300
      refreshTime: 3600
      retryTime: 300
      serialNumber: 1
    }
  }
}

resource txt_dns 'Microsoft.Network/dnszones/TXT@2018-05-01' = {
  parent: public_dns
  name: '@'
  properties: {
    TTL: 3600
    TXTRecords: [
      {
        value: [
          unique_txt
        ]
      }
    ]
  }
}


resource app_dns 'Microsoft.Network/dnszones/A@2018-05-01' = {
  parent: public_dns
  name: '@'
  properties: {
    TTL: 3600
    targetResource: {
      id: stapp.id
    }
  }
}
