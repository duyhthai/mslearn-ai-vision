import unittest
from unittest.mock import patch
from your_module import main  # Replace 'your_module' with the actual module name

class TestMain(unittest.TestCase):

    @patch('dotenv.load_dotenv')
    def test_main_loads_dotenv(self, mock_load_dotenv):
        main()
        mock_load_dotenv.assert_called_once()

    @patch.dict(os.environ, {'API_KEY': 'test_key'}, clear=True)
    def test_main_missing_api_key(self):
        with self.assertRaises(Exception) as context:
            main()
        self.assertIn("API_KEY is missing", str(context.exception))

    @patch('time.sleep')
    def test_main_sleeps_for_5_seconds(self, mock_sleep):
        main()
        mock_sleep.assert_called_once_with(5)

if __name__ == '__main__':
    unittest.main()