start:
	docker run --rm -d \
		--name axelor \
	    	--ulimit nofile=65536:65536 \
    	    	--volume ./certs:/etc/nginx/certs \
    	    	--publish 80:80 \
    	    	--publish 443:443 \
    	    	axelor/aio-erp

stop:
	docker stop axelor

exec:
	docker exec -ti axelor /bin/bash 

