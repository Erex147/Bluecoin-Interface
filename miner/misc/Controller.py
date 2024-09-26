import requests

DEBUG = True

class Controller:
    def __init__(self, base_url):
        """
        Initialize the Controller with a base URL.
        """
        self.base_url = base_url

    def post(self, endpoint, data):
        """
        Send a POST request with the provided data.

        :param endpoint: The API endpoint to send the POST request to.
        :param data: Dictionary containing the data to be sent in the POST request.
        :return: The response from the server.
        """
        try:
            url = self.base_url + endpoint
            response = requests.post(url, json=data)

            # Print response details
            if DEBUG:
                print(f"Response Code: {response.status_code}")
                print(f"Response Text: {response.text}")

            return response
        except requests.exceptions.RequestException as e:
            print(f"An error occurred: {e}")
            return None
        
    def get(self, endpoint):
        """
        Send a GET request to the provided API endpoint.

        :param endpoint: The API endpoint to send the GET request to.
        :return: The response from the server.
        """
        try:
            url = self.base_url + endpoint
            response = requests.get(url)

            # Print response details
            if DEBUG:
                print(f"Response Code: {response.status_code}")
                print(f"Response Text: {response.text}")

            return response
        except requests.exceptions.RequestException as e:
            print(f"An error occurred: {e}")
            return None

# Usage
if __name__ == "__main__":
    base_url = "https://gabserver.eu"  # Replace with your actual base URL
    controller = Controller(base_url)

    while True:
        user_input = input("\nWhat do you want to do?\n"
                           "1: Check Balance\n"
                           "2: Change Password\n"
                           "3: Create Transaction\n"
                           "4: Signup\n"
                           "5: Delete Account\n"
                           "6: List Transactions\n"
                           "7: List Users\n"
                           "q: Quit\n"
                           "Enter your choice: ")

        if user_input == "1":
            username = input("Enter username: ")
            response = controller.post('/v1/balance', {'username': username})
            if response:
                print(f"Balance: {response.json()}")

        elif user_input == "2":
            username = input("Enter username: ")
            password = input("Enter current password: ")
            new_password = input("Enter new password: ")
            data = {
                'username': username,
                'password': password,
                'new_password': new_password
            }
            response = controller.post('/v1/changepass', data)
            if response:
                print("Password changed successfully")

        elif user_input == "3":
            sender = input("Enter sender username: ")
            receiver = input("Enter receiver username: ")
            amount = int(input("Enter amount: "))
            note = input("Enter note (optional): ")
            data = {
                'sender_username': sender,
                'receiver_username': receiver,
                'amount': amount,
                'note': note
            }
            response = controller.post('/v1/create_transaction', data)
            if response:
                print("Transaction created successfully")

        elif user_input == "4":
            username = input("Enter username: ")
            password = input("Enter password: ")
            response = controller.post('/v1/signup', {'username': username, 'password': password})
            if response:
                print("Signup successful")

        elif user_input == "5":
            username = input("Enter username: ")
            password = input("Enter password: ")
            response = controller.post('/v1/delete', {'username': username, 'password': password})
            if response:
                print("Account deleted successfully")

        elif user_input == "6":
            response = controller.get('/v1/listt')
            if response:
                print(f"Transactions: {response.json()}")

        elif user_input == "7":
            response = controller.get('/v1/listu')
            if response:
                print(f"Users: {response.json()}")

        elif user_input.lower() == "q":
            print("Exiting...")
            break

        else:
            print("Invalid choice, please try again.")
