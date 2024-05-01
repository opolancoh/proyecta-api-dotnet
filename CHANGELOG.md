# Changelog

All notable changes to this project will be documented in this file.

## [0.4.0] - 2024-05-01 (Beta Release)

### Added
- DisplayName, CreatedBy and UpdatedBy to the ApplicationUser entity.
- Added user authentication with Firebase.

### Changed
- Removed RefreshToken and RefreshTokenExpiryTime from ApplicationUser entity.
- Group EF Core migrations by DbContext.

### Fixed
- Fixed a bug causing the app to crash on the profile page when no data is available.

## [0.3.0] - 2024-04-23 (Beta Release)

### Changed
- Enhance ApplicationResult.cs with Success and Code properties.

## [0.2.0] - 2024-04-11 (Beta Release)

### Added
- Added 'AllowAllOrigins' cors policy.

### Changed
- Improve system-info endpoint.

## [0.1.0] - 2024-04-10 (Beta Release)

### Added
- Initial version.