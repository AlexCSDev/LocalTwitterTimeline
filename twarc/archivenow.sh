#!/bin/bash
now=$(date +"%m_%d_%Y_%H_%M")
echo "fetching timeline"
/usr/local/bin/twarc timeline --log /opt/twarc/twarc.log > /opt/twarc/html/timeline_$now.json
echo "generating html"
/usr/bin/python3 /opt/twarc/wall.py /opt/twarc/html/timeline_$now.json > /opt/twarc/html/timeline_$now.html
echo "creating symbolic links"
rm /opt/twarc/html/timeline_latest.html
rm /opt/twarc/html/timeline_latest.json
ln -s /opt/twarc/html/timeline_$now.html /opt/twarc/html/timeline_latest.html
ln -s /opt/twarc/html/timeline_$now.json /opt/twarc/html/timeline_latest.json
echo "done"
