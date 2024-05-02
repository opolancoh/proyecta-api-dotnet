# Changelog

All notable changes to this project are documented in this file.

## [0.5.0] - 2024-05-02 (Beta Release)

### Added
- Swagger documentation.

### Changed
- Renamed `ApplicationResult` to `ApiResponse`.

## [0.4.0] - 2024-05-01 (Beta Release)

### Added
- `DisplayName`, `CreatedBy`, and `UpdatedBy` fields to the `ApplicationUser` entity.

### Changed
- Removed `RefreshToken` and `RefreshTokenExpiryTime` from the `ApplicationUser` entity.
- Grouped EF Core migrations by `DbContext`.

### Fixed
- Resolved a bug that caused the app to crash on the profile page when no data was present.

## [0.3.0] - 2024-04-23 (Beta Release)

### Changed
- Enhanced `ApplicationResult.cs` with `Success` and `Code` properties.

## [0.2.0] - 2024-04-11 (Beta Release)

### Added
- `AllowAllOrigins` CORS policy.

### Changed
- Enhanced the system-info endpoint.

## [0.1.0] - 2024-04-10 (Beta Release)

### Added
- Initial version.
