import unittest
from unittest.mock import patch, mock_open
from io import StringIO
from your_module import main  # Replace 'your_module' with the actual module name

class TestMain(unittest.TestCase):

    @patch('dotenv.load_dotenv')
    def test_main_loads_dotenv(self, mock_load_dotenv):
        main()
        mock_load_dotenv.assert_called_once()

    @patch('os.getenv')
    def test_main_raises_exception_if_os_getenv_fails(self, mock_getenv):
        mock_getenv.side_effect = EnvironmentError("Failed to get environment variable")
        with self.assertRaises(EnvironmentError) as context:
            main()
        self.assertIn("Failed to get environment variable", str(context.exception))

    @patch('matplotlib.pyplot.show')
    def test_main_calls_pyplot_show(self, mock_show):
        main()
        mock_show.assert_called_once()

if __name__ == '__main__':
    unittest.main()