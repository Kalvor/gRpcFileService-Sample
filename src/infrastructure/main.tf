resource "azurerm_resource_group" "rg" {
  location = var.resource_group_location
  name     = var.resource_group_name
}

resource "azurerm_storage_account" "sa" {
  name                     = var.storage_account_name
  resource_group_name      = azurerm_resource_group.rg.name
  location                 = azurerm_resource_group.rg.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_storage_container" "sa_container" {
  name                  = var.storage_account_container_name
  storage_account_name  = azurerm_storage_account.sa.name
  container_access_type = "private"
}

data "azurerm_storage_account_sas" "sa_data" {
  connection_string = azurerm_storage_account.sa.primary_connection_string
  https_only        = false

  resource_types {
    service   = true
    container = true
    object    = true
  }

  services {
    blob  = true
    queue = false
    table = false
    file  = false
  }

  start  = "2023-10-25T00:00:00Z"
  expiry = "2023-10-30T00:00:00Z"

  permissions {
    read    = true
    write   = true
    delete  = false
    list    = false
    add     = true
    create  = true
    update  = false
    process = false
    tag     = false
    filter  = false
  }
}

resource "azurerm_service_plan" "this" {
  name                = var.azurerm_app_service_plan_name
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  os_type             = "Linux"
  sku_name            = "F1"      
}

resource "azurerm_linux_web_app" "grpcserver" {
  name                = var.azurerm_app_service_name
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  service_plan_id = azurerm_service_plan.this.id
  site_config {
    application_stack {
      dotnet_version = "7.0"
    }
    always_on = false
    http2_enabled = true
  }
  
  app_settings = {
    "BlobStorageConnection__ContainerName" = azurerm_storage_container.sa_container.name
    "BlobStorageConnection__StorageAccountName" = azurerm_storage_account.sa.name
    "BlobStorageConnection__Url" = "https://${azurerm_storage_account.sa.name}.blob.core.windows.net"
    "BlobStorageConnection__SAS" = data.azurerm_storage_account_sas.sa_data.sas
  }
}