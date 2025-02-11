import unittest
from unittest.mock import patch
from your_module import main  # Replace 'your_module' with the actual module name

class TestMain(unittest.TestCase):

    @patch('dotenv.load_dotenv')
    def test_main_loads_environment_variables(self, mock_load_dotenv):
        mock_load_dotenv.return_value = None
        with self.assertRaises(SystemExit) as cm:
            main()
        self.assertEqual(cm.exception.code, 0)

    @patch('os.environ.get')
    def test_main_missing_environment_variable(self, mock_get_env):
        mock_get_env.side_effect = KeyError
        with self.assertRaises(SystemExit) as cm:
            main()
        self.assertEqual(cm.exception.code, 1)

    @patch('time.sleep')
    def test_main_sleeps_correctly(self, mock_sleep):
        mock_sleep.return_value = None
        with self.assertRaises(SystemExit) as cm:
            main()
        self.assertEqual(cm.exception.code, 0)

if __name__ == '__main__':
    unittest.main()