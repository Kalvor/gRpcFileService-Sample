variable "resource_group_location" {
  type        = string
  default     = "centralus"
}

variable "resource_group_name" {
  type        = string
  default     = "Rg-gRpcFileService-Sample"
}

variable "storage_account_name" {
  type        = string
  default     = "grpcfileservicesamplesa"
}

variable "storage_account_container_name" {
  type        = string
  default     = "files"
}

variable "azurerm_app_service_plan_name" {
  type        = string
  default     = "grpcfileserviceplan"
}

variable "azurerm_app_service_name" {
  type        = string
  default     = "grpcfileserviceserver"
}

