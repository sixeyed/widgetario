ARG ALPINE_VERSION="3.15"
ARG TRIVY_VERSION="0.21.1"

FROM alpine:$ALPINE_VERSION AS download-base
WORKDIR /downloads
RUN echo "$(apk --print-arch)" > /arch.txt 
RUN ARCH2= && alpineArch="$(apk --print-arch)" \
    && case "${alpineArch##*-}" in \
    x86_64) ARCH2='amd64' ;; \
    aarch64) ARCH2='arm64' ;; \
    *) echo "unsupported architecture"; exit 1 ;; \
    esac && \
    echo $ARCH2 > /arch2.txt 

FROM download-base AS docker
ARG DOCKER_CLI_VERSION="20.10.11-r0"
RUN apk add --no-cache --repository http://dl-cdn.alpinelinux.org/alpine/edge/community \
    docker-cli=$DOCKER_CLI_VERSION

FROM download-base AS kubectl
ARG KUBECTL_VERSION="1.23.1-r0"
RUN apk add --no-cache --repository http://dl-cdn.alpinelinux.org/alpine/edge/testing \
    kubectl=$KUBECTL_VERSION

FROM download-base AS buildkit
ARG BUILDKIT_VERSION="v0.9.3"
RUN wget -O buildkit.tar.gz "https://github.com/moby/buildkit/releases/download/$BUILDKIT_VERSION/buildkit-$BUILDKIT_VERSION.linux-$(cat /arch2.txt).tar.gz"
RUN tar xvf buildkit.tar.gz

FROM download-base AS helm
ARG HELM_VERSION="v3.7.1"
RUN wget -O helm.tar.gz "https://get.helm.sh/helm-$HELM_VERSION-linux-$(cat /arch2.txt).tar.gz"
RUN tar xvf helm.tar.gz --strip-components 1

FROM download-base AS compose
ARG COMPOSE_VERSION="v2.2.0"
RUN wget -O docker-compose "https://github.com/docker/compose/releases/download/$COMPOSE_VERSION/docker-compose-linux-$(cat /arch.txt)"

FROM aquasec/trivy:$TRIVY_VERSION AS trivy

FROM download-base AS jenkins
ARG JENKINS_VERSION="2.319.1"
RUN wget http://mirrors.jenkins.io/war-stable/$JENKINS_VERSION/jenkins.war

# Jenkins
FROM alpine:$ALPINE_VERSION

# jenkins deps
RUN apk add --no-cache \
    bash \
    coreutils \
    jq \
    git \
    openjdk11 \
    openssh-client \
    ttf-dejavu \
    unzip 
    
ENV JENKINS_HOME="/data"
VOLUME ${JENKINS_HOME}

EXPOSE 8080
ENTRYPOINT /start.sh

COPY --from=kubectl /usr/bin/kubectl /usr/bin/kubectl
COPY --from=docker /usr/bin/docker /usr/bin/docker
COPY --from=buildkit /downloads/bin/buildctl /usr/bin/buildctl
COPY --from=helm /downloads/helm /usr/bin/helm
COPY --from=compose /downloads/docker-compose /root/.docker/cli-plugins/docker-compose
COPY --from=trivy /usr/local/bin/trivy /usr/local/bin/trivy
COPY --from=jenkins /downloads/jenkins.war /jenkins/jenkins.war

COPY ./jenkins.install.UpgradeWizard.state ${JENKINS_HOME}/
COPY ./scripts/ ${JENKINS_HOME}/init.groovy.d/
COPY start.sh /

RUN chmod +x /start.sh && \
    chmod +x /root/.docker/cli-plugins/docker-compose