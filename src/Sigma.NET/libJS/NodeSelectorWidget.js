// Internal state
const state = {};

function setHoveredNode(node) {
    if (node) {
    state.hoveredNode = node;
    state.hoveredNeighbors = new Set(graph.neighbors(node));
    } else {
    state.hoveredNode = undefined;
    state.hoveredNeighbors = undefined;
    }
    // Refresh rendering:
    renderer.refresh();
}


// Bind graph interactions:
renderer.on("enterNode", ({ node }) => {
    setHoveredNode(node);
});
renderer.on("leaveNode", () => {
    setHoveredNode(undefined);
});

// Render nodes accordingly to the internal state:
// 1. If a node is selected, it is highlighted            
// 2. If there is a hovered node, all non-neighbor nodes are greyed
renderer.setSetting("nodeReducer", (node, data) => {
    //const res: Partial<NodeDisplayData> = { ...data };
    const res = data;

    if (state.hoveredNeighbors && !state.hoveredNeighbors.has(node) && state.hoveredNode !== node) {
    res.label = "";
    res.color = "#f6f6f6";
    }

    return res;
});

// Render edges accordingly to the internal state:
// 1. If a node is hovered, the edge is hidden if it is not connected to the
//    node
renderer.setSetting("edgeReducer", (edge, data) => {
    //const res: Partial<EdgeDisplayData> = { ...data };
    const res = data;

    if (state.hoveredNode && !graph.hasExtremity(edge, state.hoveredNode)) {
    res.hidden = true;
    }

    return res;
});  

// Minified Javascript
//const state={};function setHoveredNode(e){e?(state.hoveredNode=e,state.hoveredNeighbors=new Set(graph.neighbors(e))):(state.hoveredNode=void 0,state.hoveredNeighbors=void 0),renderer.refresh()}renderer.on("enterNode",({node:e})=>{setHoveredNode(e)}),renderer.on("leaveNode",()=>{setHoveredNode(void 0)}),renderer.setSetting("nodeReducer",(e,t)=>{let o=t;return state.hoveredNeighbors&&!state.hoveredNeighbors.has(e)&&state.hoveredNode!==e&&(o.label="",o.color="#f6f6f6"),o}),renderer.setSetting("edgeReducer",(e,t)=>{let o=t;return state.hoveredNode&&!graph.hasExtremity(e,state.hoveredNode)&&(o.hidden=!0),o});