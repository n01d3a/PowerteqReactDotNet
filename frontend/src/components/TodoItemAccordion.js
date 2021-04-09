import React, { useEffect, useState } from "react";
import { Accordion, AccordionDetails, AccordionSummary, Button, Grid, TextField } from "@material-ui/core";
import { Delete as DeleteIcon, Edit as EditIcon, ExpandMore as ExpandMoreIcon } from "@material-ui/icons";


export default function TodoItemAccordion(props) {
    const [expandable, setExpandable] = useState(false);
    const [expanded, setExpanded] = useState(false);
    useEffect(() => {
        const canExpand = !!props.todoItem.description;
        setExpandable(canExpand);
        setExpanded(canExpand && expanded);
    }, [props.todoItem, expanded]);

    const expandedChanged = (event, newExpandedState) => {
        setExpanded(expandable && newExpandedState);
    }

    return (
        <Accordion expanded={expanded} onChange={expandedChanged}>
            <AccordionSummary expandIcon={expandable ? <ExpandMoreIcon /> : <ExpandMoreIcon color="disabled" />} >
                <Grid container spacing={1}>
                    <Grid item xs >
                        <TextField value={props.todoItem.title} helperText={`Created: ${props.todoItem.createdDate.format("YYYY-MM-DD HH:mm:ss")}`} InputProps={{ readOnly: true, disableUnderline: true }} />
                    </Grid>
                    <Grid item xs="auto">
                        <Button startIcon={<EditIcon />} variant="contained" color="secondary" onClick={event => { event.stopPropagation(); props.onEditClicked(props.todoItem); }} onFocus={event => event.stopPropagation()}>Edit</Button>
                    </Grid>
                    <Grid item xs="auto">
                        <Button startIcon={<DeleteIcon />} variant="contained" color="secondary" onClick={event => { event.stopPropagation(); props.onDeleteClicked(props.todoItem); }} onFocus={event => event.stopPropagation()}>Delete</Button>
                    </Grid>
                    <Grid item xs={1} />
                </Grid>
            </AccordionSummary>
            <AccordionDetails>
                <TextField value={props.todoItem.description} InputProps={{ readOnly: true, disableUnderline: true }} margin="dense" fullWidth multiline />
            </AccordionDetails>
        </Accordion >
    );
}