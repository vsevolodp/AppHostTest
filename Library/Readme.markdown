Lokad.Cloud Application Host
============================

Prototype for hosting Lokad.Cloud services or other applications in Windows Azure or on premises.

Highlights:

* Host to run your applications in multiple isolated cells in their own Thread and AppDomain.
* Fast automated deployments, switching between deployments by changing a single blob.
* Git-like deployment versioning scheme. Easily get back to a known working version, including settings.
* Continuous deployments, e.g. avoiding unloading AppDomains if only settings or only assemblies of one cell have changed.
* No dependencies except SharpZipLib (no IoC container, no Azure SDK, no Lokad.Cloud storage)
* Simple. No complicated magic happening. Opensource and easy to extend to your demands. Available in the NuGet Gallery.

Deployments
-----------

    <Deployment>
      <Cells>
        <Cell name="default">
          <Assemblies name="assemblies-sha256hex" />
          <EntyPoint typeName="typeName, assemblyName" />
          <Settings>... optional, user defined ...</Settings>
        </Cell>
        <Cell name="someLokadCloudServicesCell">
          <Assemblies name="assemblies-abf563fea56543aa" />
          <Settings>
            <Config name="config-ab7875978deca56" />
            <Services name="services-89054dea46c43a" />
          </Settings>
        </Cell>
      </Cells>
    </Deployment>

A deployment can define one or more named cells which will run on the same VM but isolated by AppDomain and in their own threads.

Each cell must specify the name of the assemblies package (essentialy a zip file containing all assemblies). Assemblies can be shared between cells, so you have the same cell (except the cell name) multiple times. Assemblies are expected to be named after their content hash, ideally SHA256 or SHA-1, so that assemblies associated with a name are guaranteed to never change.

If your cell does not run Lokad.Cloud services you need to specify your cell entry point type name in the EntryPoint-tag. EntryPoints must implement the IApplicationEntryPoint interface.

Optionally you can add arbitrary configuration in the Settings tag for each cell. This XElement will be provided to your CellRunner.

Similar to assemblies, the deployment blob is expected to be named after its content hash (SHA256 or SHA-1). A deployment can therefore never change, i.e. any change will lead to a new deployment with a new name.

Head Deployment
---------------

    <Head>
      <Deployment name="deployment-abdd565abf23" />
    </Head>

HEAD is essentially a pointer to the current deployment. The AppHost polls this document for changes and loads the referenced deployment if a change is detected.

Running the AppHost
-------------------

E.g. in an Azure Worker Role, using the host context implementation provided by the Lokad.Cloud Services Framework:

    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource _cts;
        private readonly Host _host;

        public WorkerRole()
        {
            _cts = new CancellationTokenSource();
            var context = LokadCloudHostContext.CreateFromRoleEnvironment();
            _host = new Host(context);
        }

        public override void Run()
        {
            _host.RunSync(_cts.Token);
        }

        public override void OnStop()
        {
            _cts.Cancel();
        }
    }