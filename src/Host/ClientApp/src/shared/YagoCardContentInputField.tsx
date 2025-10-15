import { Box, TextField } from "@mui/material";

interface YagoCardContentInputFieldProps {
    name: string,
    handleChange: (e: React.ChangeEvent<HTMLInputElement>) => void,
    error: string
}

const YagoCardSContentInputField: React.FC<YagoCardContentInputFieldProps> = ({ name, handleChange, error }) => {
    return (
        <Box mx={2} textAlign="center">
            <Box mb={2}>
                <TextField
                    fullWidth
                    label="Название колонии"
                    value={name}
                    onChange={handleChange}
                    error={!!error}
                    helperText={error}
                    sx={{ mb: 2 }} />
            </Box>
        </Box>
    );
}

export default YagoCardSContentInputField