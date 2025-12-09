
def call(String path, int port){
    sh """
        cd ${path}
        docker build -t myservice:latest .
        docker run -d -p ${port}:${port} myservice:latest
    """
} 