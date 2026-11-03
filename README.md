# OhrTech Developers

The source of [docs.ohrtech.net](https://docs.ohrtech.net): the documentation for the OhrTech SDK,
in two parts.

- **Management API** (`src/content/docs/management-api/`): the cloud API at `api.ohrtech.net`, for
  your servers and tools.
- **Device SDK** (`src/content/docs/device-sdk/`): for apps running on OhrTech devices. Coming soon.

Built with [Astro Starlight](https://starlight.astro.build) and [Scalar](https://scalar.com) for the
API reference, and hosted on Cloudflare Pages: every push to `main` deploys.

## Work on it

```sh
npm install
npm run dev        # http://localhost:4321
npm run build      # also checks every internal link
```

## Code that can't go stale

- `examples/*.cs` are complete .NET 10 programs. The recipe pages show them as they are, and CI builds
  every one against the published `OhrTech.Management` package.
- Every C# snippet written into a page is compiled too: `node scripts/check-snippets.mjs` collects
  them into `examples/.page-snippets.cs`. Mark one that isn't meant to compile alone with
  ```` ```csharp nocheck ````.
- `npm run sync-openapi` copies the API's OpenAPI document from
  [OhrTech/management-api](https://github.com/OhrTech/management-api) into
  `public/management-api/openapi.json` and adds the C# SDK sample for each operation, also written
  to `examples/reference-samples.cs` for CI to compile. Run it whenever the API changes.

## License

[MIT](LICENSE).
