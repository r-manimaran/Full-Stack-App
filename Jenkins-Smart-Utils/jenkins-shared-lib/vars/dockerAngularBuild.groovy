def call(String imageName, String path){
    sh """
        cd Jenkins-Smart-Utils/MicroFrontends
        docker build -f ${path}/Dockerfile -t ${imageName}:latest .
    """
}