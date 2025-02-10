import unittest
from unittest.mock import patch, mock_open
from io import StringIO
from your_module import main  # Replace 'your_module' with the actual module name

class TestMain(unittest.TestCase):

    @patch('dotenv.load_dotenv')
    @patch.dict(os.environ, {'API_KEY': 'test_key'})
    def test_main_with_valid_api_key(self, mock_load_dotenv):
        with patch('requests.get') as mock_get:
            mock_get.return_value.status_code = 200
            mock_get.return_value.json.return_value = {'key': 'value'}
            
            with patch('matplotlib.pyplot.show'):
                main()
                
            mock_get.assert_called_once_with('https://api.example.com/data?key=test_key')

    @patch('dotenv.load_dotenv')
    @patch.dict(os.environ, {'API_KEY': ''})
    def test_main_with_empty_api_key(self, mock_load_dotenv):
        with patch('requests.get') as mock_get:
            mock_get.return_value.status_code = 200
            mock_get.return_value.json.return_value = {'key': 'value'}
            
            with patch('matplotlib.pyplot.show'):
                main()
                
            mock_get.assert_not_called()

    @patch('dotenv.load_dotenv')
    @patch.dict(os.environ, {'API_KEY': None})
    def test_main_with_none_api_key(self, mock_load_dotenv):
        with patch('requests.get') as mock_get:
            mock_get.return_value.status_code = 200
            mock_get.return_value.json.return_value = {'key': 'value'}
            
            with patch('matplotlib.pyplot.show'):
                main()
                
            mock_get.assert_not_called()

if __name__ == '__main__':
    unittest.main()