package wiredbrain.products;

import io.micrometer.core.annotation.Timed;
import io.micrometer.core.instrument.MeterRegistry;

import java.math.BigDecimal;
import java.util.Arrays;
import java.util.List;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.client.RestTemplate;

@RestController
public class ProductsController {
    private static final Logger log = LoggerFactory.getLogger(ProductsController.class);
    
    @Autowired
	ProductRepository repository;

    @Autowired
    MeterRegistry registry;

    @RequestMapping("/products")
    @Timed()
    public List<Product> get() {
        log.debug("** GET /products called");
        registry.counter("products_data_load_total", "status", "called").increment();
        List<Product> products = null;
        try { 
            products = repository.findAll();
        }
        catch (Exception ex) {
            log.debug("** GET /products failed!");
            registry.counter("products_data_load_total", "status", "failure").increment();
        }        
        return products;
    }
}
