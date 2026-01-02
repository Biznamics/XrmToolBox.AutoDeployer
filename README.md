# Auto Deployer Tool in [XrmToolBox](https://www.xrmtoolbox.com)

Auto Deployer is an **XrmToolBox plugin** that watches local files and automatically deploys updates to **Dataverse / Dynamics 365**.

It currently supports:

- **Plugin Assembly auto-update** (watch a `.dll`, update the plugin assembly when the file changes)
- **Web Resource auto-deploy** (watch selected local files and update/publish the matching Dataverse Web Resources)

---

## Features

### ✅ Plugin Assembly auto-deploy
- Watch one or more plugin assembly `.dll` files
- When the file changes, the tool updates the corresponding Plugin Assembly in Dataverse
- Logs actions and results in the tool output pane

### ✅ Web Resource auto-deploy
- Configure:
  - **Root folder** (local build output folder)
  - **Prefix** (Dataverse web resource prefix, e.g. `ifse_`)
  - **Patterns** (file globs like `scripts\*.js`)
  - **Publish after update** (on/off)
  - **Debounce** (ms) to batch rapid rebuilds
- Scan local folder to discover files that match the patterns
- Map each file to a Web Resource name (editable)
- Watch only mappings where **Watch = true**
- Upload updated content on change, and optionally publish

---

## Requirements

- XrmToolBox (latest recommended)
- A Dataverse / Dynamics 365 connection with permissions to:
  - Update Plugin Assemblies (for plugin mode)
  - Update Web Resources + Publish (for web resource mode)

---

## Getting Started

### 1) Install / Run in XrmToolBox
- Build the project
- Copy the plugin output to your XrmToolBox Plugins folder (or use your normal XrmToolBox dev workflow)
- Start XrmToolBox and open **Auto Deployer**

### 2) Connect to an environment
The tool is connection-aware. Configuration is stored **per connection**.

---

## Using Plugin Assembly Watching

1. Click **Add** → **Plugin Assembly**
2. Select a plugin `.dll`
3. The tool starts watching the file
4. Rebuild your project → when the `.dll` changes, Auto Deployer updates the plugin assembly
5. Select an item in the list to see detailed logs

> Tip: Keep your build output stable (same path) so the watcher doesn’t need to be reconfigured.

---

## Using Web Resource Watching

1. Click **Add** → **Web Resources**
2. Configure:
   - **Root folder**  
     Example: `C:\Git\MySolution\dist\gdpr`
   - **Prefix**  
     Example: `ifse_`
   - **Patterns** (one per line)  
     Examples:
     ```
     scripts\*.js
     content\*.css
     images\*.png
     ```
   - **Publish after update** (recommended ON)
   - **Debounce** (default 1500ms is a good start)

3. Click **Scan**
   - Adds matching local files to the grid (without duplicates)
   - Auto-generates `CrmName` as: `{prefix}/{relativePath}`

4. Set **Watch = true** for the rows you want actively monitored

5. Click **Save**
   - Configuration is saved for the current connection
   - Watchers are created for rows where **Watch = true**

---

## How Web Resource Mapping Works

For each watched mapping:

- **RelativePath** is the local path relative to Root folder  
  Example: `scripts\ifse_gdpr_consent.js`

- **CrmName** is the Dataverse Web Resource name  
  Example: `ifse_/scripts/ifse_gdpr_consent.js`

On file change, the tool:
1. Uploads the new content to that Web Resource
2. Optionally publishes (if enabled)
3. Logs result to the main log pane

---

## Configuration & Persistence

Settings are stored using the standard XrmToolBox settings mechanism:

- Saved **per connection** (so each environment has its own config)
- Loaded automatically when the connection changes
- Web Resource watchers are re-created when you connect (based on saved config)

---

## Troubleshooting

### “Not connected to an environment”
Connect in XrmToolBox first. Web Resource deployment requires an active service connection.

### Nothing happens when files change
- Ensure the row has **Watch = true**
- Check that **Root folder** is correct and file really exists under it
- Some build processes replace files via temp rename; try increasing **Debounce**
- Verify permissions to update web resources / publish

### Web resource name doesn’t match what’s in Dataverse
Edit the **CrmName** column to match the exact Dataverse web resource name.
The tool does not guess your naming beyond `{prefix}/{relativePath}`.

### Large rebuilds cause multiple deployments
Increase **Debounce ms** (e.g. 2000–5000ms) to batch changes.

---

## Roadmap / Ideas

- Validate button enhancements (existence checks in Dataverse, type checks, etc.)
- Bulk enable/disable watch flags
- Better “summary” view in MainControl (active watchers / config overview)
- Optional “dry run” mode for web resources
- Conflict handling / ETag support (if needed)

---

## Contributing

PRs and issues are welcome.

If you contribute:
- Keep changes backward compatible where possible
- Prefer small PRs focused on a single improvement
- Include short notes in the PR description on how to test

---

## License

Add your preferred license here (MIT / Apache-2.0 / etc.).
