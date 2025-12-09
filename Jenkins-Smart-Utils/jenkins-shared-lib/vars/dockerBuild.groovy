def call(String imageName, string path){
    sh """
        cd ${path}
        docker build -t ${imageName}:latest .
    """
}