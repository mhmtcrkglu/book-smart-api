# Book Smart API

## About the Project

Book Smart API is a system that facilitates appointment bookings for customers. Customers can view and reserve one-hour
appointment slots through a website. The system organizes these slots according to specific rules and matches customers
with suitable sales managers based on certain traits.

## Technologies Used

- **.NET Core 8.0:** A platform for developing modern web applications.
- **PostgreSQL:** A powerful relational database management system.
- **Docker:** Used to run application containers in an isolated environment.

## Running the Project with Docker

### Requirements

- **Docker:** You need Docker installed on your computer to run the project. You can download Docker from
  the [official Docker website](https://www.docker.com/get-started).

### Running with Docker Compose

To run the project with Docker Compose:

1. **(Optional) Edit Docker Compose File:**

   Check and edit the `docker-compose.yml` file in the project root directory if necessary.

2. **Start the Project:**

   Navigate to the project root directory in your terminal and run:

   ```bash
   docker-compose up --build
   ```

3. **Using the Project:**

    - **API Access:** Access the API at [http://localhost:3000](http://localhost:3000).

4. **Stopping Containers:**

   ```bash
   docker-compose down
   ```

## API Endpoint Details

### Slot Query

- **URL:** `POST http://localhost:3000/calendar/query`
- **Input (POST Body):**

  ```json
  {
    "date": "2024-05-03",
    "products": ["SolarPanels", "Heatpumps"],
    "language": "German",
    "rating": "Gold"
  }
  ```

- **Output:**

  ```json
  [
    {
      "start_date": "2024-05-03T10:30:00.000Z",
      "available_count": 1
    },
    {
      "start_date": "2024-05-03T11:00:00.000Z",
      "available_count": 1
    },
    {
      "start_date": "2024-05-03T11:30:00.000Z",
      "available_count": 1
    }
  ]
  ```

- **Note:** All fields in the request body are considered mandatory, and the endpoint is designed based on this
  assumption.

## Database Performance and Indexes

- **Indexes for `sales_managers` Table:**
    - **`idx_sales_managers_languages`**: GIN index on the `languages` column.
    - **`idx_sales_managers_customer_ratings`**: GIN index on the `customer_ratings` column.
    - **`idx_sales_managers_products`**: GIN index on the `products` column.

- **Index for `slots` Table:**
    - **`idx_slots_sales_manager_start_date_booked`**: Covers `sales_manager_id`, `start_date`, and `booked` columns.

## Notes

- **Performance Improvements:** The added indexes are designed to improve the performance of specific queries. To make
  the most of these indexes, ensure your queries are optimized to use them effectively.
- **Database Setup:** The `init.sql` file contains database configurations and initial data. Make sure it works
  correctly by checking that `init.sql` is properly executed when you start the database container with Docker Compose.