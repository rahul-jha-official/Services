# RabbitMQ
RabbitMQ is an open-source message broker that enables applications to communicate with each other by sending messages. It implements the Advanced Message Queuing Protocol (AMQP) for message queuing. RabbitMQ is widely used in microservices architectures to enable asynchronous communication between services.


RabbitMQ is a message broker: it accepts and forwards messages. You can think about it as a post office: when you put the mail that you want posting in a post box, you can be sure that the letter carrier will eventually deliver the mail to your recipient. In this analogy, RabbitMQ is a post box, a post office, and a letter carrier.

The major difference between RabbitMQ and the post office is that it doesn't deal with paper, instead it accepts, stores, and forwards binary blobs of data - messages.

<img src="https://github.com/user-attachments/assets/53d336c5-f0a8-401d-9ab5-da4fc38c2252">


RabbitMQ, and messaging in general, uses some jargon.
- Producing means nothing more than sending. A program that sends messages is a producer.
- A queue is the name for the post box in RabbitMQ. Although messages flow through RabbitMQ and your applications, they can only be stored inside a queue. A queue is only bound by the host's memory & disk limits, it's essentially a large message buffer. Many producers can send messages that go to one queue, and many consumers can try to receive data from one queue.
- Consuming has a similar meaning to receiving. A consumer is a program that mostly waits to receive messages.

## Key Concepts
- **Broker**: RabbitMQ server that manages queues and exchanges.
- **Queue**: A buffer that stores messages until they are processed by consumers.
- **Exchange**: Routes messages to one or more queues based on routing rules.
- **Binding**: A link between an exchange and a queue that defines how messages are routed.
- **Producer**: An application that sends messages to an exchange.
- **Consumer**: An application that receives messages from a queue.

## Exchange
The core idea in the messaging model in RabbitMQ is that the producer never sends any messages directly to a queue. Actually, quite often the producer doesn't even know if a message will be delivered to any queue at all.

Instead, the producer can only send messages to an exchange. An exchange is a very simple thing. On one side it receives messages from producers and the other side it pushes them to queues. The exchange must know exactly what to do with a message it receives. Should it be appended to a particular queue? Should it be appended to many queues? Or should it get discarded. The rules for that are defined by the exchange type.

### Types of RabbitMQ Exchanges
RabbitMQ supports several types of exchanges, each serving different routing needs:

### Direct Exchange
- **Purpose**: Routes messages to queues based on a routing key.
- **Use Case**: Direct routing of messages to a specific queue. Ideal for scenarios where you want to route messages with a specific routing key to a particular queue.
- **Example**: A logging system where logs of different severity levels (info, error) are routed to different queues.

	```
	{
	  "exchangeType": "direct",
	  "routingKey": "info",
	  "queueName": "info-logs"
	}
	```

<img src="https://github.com/user-attachments/assets/70e32b49-9bf5-4753-b92c-64bd14aae5d8">

### Fanout Exchange
- **Purpose**: Routes messages to all bound queues regardless of the routing key.
- **Use Case**: Broadcasting messages to multiple queues. Useful for scenarios where you want to send the same message to multiple consumers.
- **Example**: A notification system where notifications are sent to multiple users.
	
	```
	{
	  "exchangeType": "fanout",
	  "queueName": "user-notifications"
	}
	```

<img src="https://github.com/user-attachments/assets/9e244222-0c98-42bb-87e8-cc47b91667b6">

### Topic Exchange
- **Purpose**: Routes messages to queues based on wildcard patterns in the routing key.
- **Use Case**: Flexible routing based on patterns. Ideal for scenarios where you want to route messages based on multiple criteria.
- **Example**: A news distribution system where articles are categorized by topics (sports, politics) and subtopics (football, elections).
	
	```
	{
	  "exchangeType": "topic",
	  "routingKey": "news.sports.football",
	  "queueName": "sports-football-news"
	}
	```

<img src="https://github.com/user-attachments/assets/8ae9de43-f7bf-472b-8078-ad4efe45ff74">

### Headers Exchange
- **Purpose**: Routes messages based on header attributes instead of routing keys.
- **Use Case**: Routing based on message attributes. Useful for scenarios where routing decisions are made based on multiple attributes rather than a single routing key.
- **Example**: A product catalog system where products are categorized by attributes like category and brand.
	
	```
	{
	  "exchangeType": "headers",
	  "headers": {
		"category": "electronics",
		"brand": "apple"
	  },
	  "queueName": "electronics-apple-products"
	}
	```

<img src="https://github.com/user-attachments/assets/62b84cda-3f61-402d-a135-2a53b4a39bc5">



