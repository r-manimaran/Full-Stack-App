
def call(String path, String projectFile) {
    sh """
        cd ${path}
        dotnet restore ${projectFile}
        dotnet build ${projectFile} --configuration Release 
        dotnet publish ${projectFile} --configuration Release --output published
    """
}