#!/bin/bash
./linux-publish.sh
cf push -f Manifest.yml 
