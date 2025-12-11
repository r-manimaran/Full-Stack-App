
def call(String path, String projectFile) {
    sh """
        cd ${path}
        dotnet restore ${projectFile}
        dotnet build --configuration Release
        dotnet publish --configuration Release --output published
    """
}