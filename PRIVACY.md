# Privacy Policy

**Last Updated: February 7, 2026**

## Overview

WhoopNet is a .NET library that provides developers with an interface to interact with the WHOOP API. This privacy policy explains the data handling practices of the WhoopNet library itself.

## Data Collection and Storage

**WhoopNet does not collect, store, or exchange any personal data.**

The library functions solely as a client-side wrapper for the WHOOP API and:

- Does not maintain any databases or storage systems
- Does not cache or persist user data
- Does not transmit data to any servers other than the official WHOOP API endpoints
- Does not log, track, or retain any personal information

## How the Library Works

WhoopNet is a pass-through library that:

1. Accepts API credentials and access tokens provided by developers
2. Constructs and sends HTTP requests to the WHOOP API on behalf of the developer
3. Returns API responses directly to the calling application
4. Operates entirely within the developer's application runtime environment

## Developer Responsibilities

Developers who integrate WhoopNet into their applications are responsible for:

- Complying with WHOOP's Terms of Service and API usage policies
- Implementing their own privacy policies for their applications
- Handling user data in accordance with applicable privacy laws and regulations (e.g., GDPR, CCPA)
- Obtaining necessary user consents for accessing WHOOP data
- Securing OAuth tokens and API credentials
- Managing data storage, retention, and deletion in their own applications

## Third-Party Services

When using WhoopNet, data is transmitted directly to WHOOP's API services. Please refer to:

- [WHOOP Privacy Policy](https://www.whoop.com/privacy-policy/)
- [WHOOP Developer Portal](https://developer.whoop.com/)

for information about how WHOOP handles personal data.

## Open Source

WhoopNet is an open-source library. The complete source code is available on GitHub at:
[https://github.com/TonySkorik/whoop-net](https://github.com/TonySkorik/whoop-net)

Users and developers can inspect the code to verify that the library does not collect or store any personal data.

## Security

While WhoopNet itself does not store data, developers should:

- Store OAuth tokens and API credentials securely
- Use HTTPS for all API communications (enforced by default)
- Follow security best practices when handling user data in their applications
- Keep the library updated to benefit from security improvements

## Changes to This Policy

This privacy policy may be updated from time to time. Any changes will be reflected in the repository with an updated "Last Updated" date.

## Contact

For questions or concerns about this privacy policy, please:

- Open an issue on the [GitHub repository](https://github.com/TonySkorik/whoop-net/issues)
- Contact the repository maintainer through GitHub

## License

This library is provided under the MIT License. See the LICENSE file in the repository for details.

---

**Note**: This privacy policy applies only to the WhoopNet library itself. Applications that use this library must provide their own privacy policies covering their specific data handling practices.
